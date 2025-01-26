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
using System.Security.Principal;
using System.Text;
using WorkHunter.Data;
using WorkHunter.Models.Config;
using WorkHunter.Models.Dto.Users;
using WorkHunter.Models.Entities;
using WorkHunter.Models.Views.Users;

public sealed class UserService : IUserService
{
    private readonly UserManager<User> userManager;
    private readonly IPrincipal currentUserPrincipal;
    private readonly AuthOptions authOptions;
    private readonly IWorkHunterDbContext dbContext;
    private readonly IValidator<LoginDto> loginValidator;

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