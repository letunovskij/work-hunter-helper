using WorkHunter.Models.Dto.Users;
using WorkHunter.Models.Views.Users;

namespace Abstractions.Users;

public interface IUserService
{
    Task<UserBaseView> GetCurrent();

    Task<TokensView> Login(LoginDto dto);

    Task<IReadOnlyList<UserBaseView>> GetAll();
}
