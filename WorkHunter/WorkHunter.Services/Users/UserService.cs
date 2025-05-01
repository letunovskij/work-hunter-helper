using Abstractions.Users;
using Common.Exceptions;
using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using WorkHunter.Data;
using WorkHunter.Models.Config;
using WorkHunter.Models.Dto.Users;
using WorkHunter.Models.Entities.Users;
using WorkHunter.Models.Views.Users;

public sealed class UserService : IUserService
{
    private readonly UserManager<User> userManager;
    private readonly IPrincipal currentUserPrincipal;
    private readonly AuthOptions authOptions;
    private readonly IWorkHunterDbContext dbContext;
    private readonly IValidator<LoginDto> loginValidator;

    private const string upper = "QWERTYUIOPASDFGHJKLZXCVBNM";
    private const string lower = "qwertyuaiopsdfgzhjklxcvbnm";
    private const string digit = "1234567890";
    private const string special = "!@#$%^&*(){}:|?><,/?:%;№!";

    public UserService(
        UserManager<User> userManager,
        IPrincipal currentUserPrincipal,
        IOptionsSnapshot<AuthOptions> authOptions,
        IWorkHunterDbContext dbContext,
        IValidator<LoginDto> loginValidator,
        IValidator<RefreshTokenDto> refreshTokenValidator
        )
    {
        this.userManager = userManager;
        this.currentUserPrincipal = currentUserPrincipal;
        this.authOptions = authOptions.Value;
        this.dbContext = dbContext;
        this.loginValidator = loginValidator;
    }

    public async Task<IReadOnlyList<UserBaseView>> GetAll()
    {
        var users = await dbContext.Users
                                   .AsNoTracking()
                                   .ToListAsync();

        return users.Adapt<List<UserBaseView>>();
    }

    public async Task<UserBaseView> GetCurrent()
    {
        if (string.IsNullOrEmpty(currentUserPrincipal.Identity?.Name))
            throw new UnauthorizedAccessException("Текущий пользователь не авторизован!");

        return await GetByUserName(currentUserPrincipal.Identity.Name);
    }

    public async Task<TokensView> Login(LoginDto dto)
    {
        loginValidator.Validate(dto);
        var user = await userManager.FindByEmailAsync(dto.Email)
            ?? throw new ArgumentException("Неверный логин или пароль!");

        CheckPossibilityToLoginAtUser(user);

        if (!await userManager.CheckPasswordAsync(user, dto.Password))
            throw new ArgumentException("Неверный логин или пароль!");
        
        return await GenerateTokens(user);
    }

    public async Task<UserView> GetById(string userId)
    {
        return await dbContext.Users
                              .ProjectToType<UserView>()
                              .SingleOrDefaultAsync(x => x.Id == userId)
                               ?? throw new EntityNotFoundException(userId, nameof(User));
    }

    public async Task<UserView> Create(UserCreateDto dto)
    {
        var user = new User()
        {
            Name = dto.Name,
            UserName = dto.UserName,
            Email = dto.Email,
        };

        await CreateUser(user, GenerateRandomPassword(), dto.Roles);

        await dbContext.SaveChangesAsync();

        return await this.GetById(user.Id);
    }

    private string GenerateRandomPassword(PasswordOptions? options = null)
    {
        options ??= userManager.Options.Password;

        string[] charsSource = new[] { upper, lower, special, digit };
        StringBuilder password = new(options.RequiredLength);

        if (options.RequireUppercase)
            password.Append(charsSource[0][RandomNumberGenerator.GetInt32(charsSource[0].Length)]);

        if (options.RequireLowercase)
            password.Append(charsSource[1][RandomNumberGenerator.GetInt32(charsSource[1].Length)]);

        if (options.RequireNonAlphanumeric)
            password.Append(charsSource[2][RandomNumberGenerator.GetInt32(charsSource[2].Length)]);

        if (options.RequireDigit)
            password.Append(charsSource[3][RandomNumberGenerator.GetInt32(charsSource[3].Length)]);

        for (int i = password.Length; i <= options.RequiredLength || charsSource.Distinct().Count() < options.RequiredUniqueChars; i++)
        {
            string nextChar = charsSource[RandomNumberGenerator.GetInt32(charsSource.Length)];
            password.Append(nextChar);
        }

        return password.ToString();
    }

    private async Task CreateUser(User user, string password, IReadOnlyList<string> roles)
    {
        var userResult = await userManager.CreateAsync(user, password);
        if (!userResult.Succeeded)
            throw new BusinessErrorException(string.Join(" ,", userResult.Errors.Select(x => x.Description)));

        if (roles?.Count > 0)
        {
            var roleResult = await userManager.AddToRolesAsync(user, roles);

            if (!roleResult.Succeeded)
                throw new BusinessErrorException(string.Join(" ,", roleResult.Errors.Select(x => x.Description)));
        }
    }

    #region private

    private async Task<UserView> GetByUserName(string userName)
    {
        var user = await GetUser(userName) ?? throw new EntityNotFoundException($"Пользователь {userName} не обнаружен!");
        return user.Adapt<UserView>();
    }

    private Task<User?> GetUser(string? userName)
        => dbContext.Users
                    .Include(x => x.UserRoles)
                        .ThenInclude(x => x.Role)
                    .FirstOrDefaultAsync(x => x.UserName == userName);

    private void CheckPossibilityToLoginAtUser(User user)
    {
        if (user.IsDeleted)
            throw new UnauthorizedAccessException("Пользователь удален!");
    }

    private async Task<TokensView> GenerateTokens(User user)
    {
        var userRoles = await userManager.GetRolesAsync(user);
        var expiresIn = DateTime.UtcNow.AddSeconds(authOptions.AccessTokenLifetime);
        var refreshExpiresIn = DateTime.UtcNow.AddSeconds(authOptions.RefreshTokenLifetime);
        var tokenHandler = new JwtSecurityTokenHandler();
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName)
        };
        if (userRoles.Count > 0)
            claims.AddRange(userRoles.Select(r => new Claim(ClaimTypes.Role, r)));
        var accessSecurityToken = new JwtSecurityToken(
            issuer: authOptions.Issuer,
            audience: authOptions.Audience,
            claims: claims,
            expires: expiresIn,
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authOptions.Key)), SecurityAlgorithms.HmacSha256Signature)
        );
        var refreshSecurityToken = new JwtSecurityToken(
            issuer: authOptions.Issuer,
            audience: authOptions.Audience,
            claims: claims,
            expires: refreshExpiresIn,
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authOptions.Key)), SecurityAlgorithms.HmacSha256Signature)
            );

        var accessToken = tokenHandler.WriteToken(accessSecurityToken);
        var refreshToken = tokenHandler.WriteToken(refreshSecurityToken);
        return new TokensView
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    #endregion
}