using WorkHunter.Models.Dto.Users;
using WorkHunter.Models.Views.Users;

namespace Abstractions.Users;

public interface IUserService
{
    Task<UserBaseView> GetCurrent();

    Task<TokensView> Login(LoginDto dto);

    Task<UserView> GetById(string userId);

    Task<IReadOnlyList<UserBaseView>> GetAll();

    Task<UserView> Create(UserCreateDto dto);
}
