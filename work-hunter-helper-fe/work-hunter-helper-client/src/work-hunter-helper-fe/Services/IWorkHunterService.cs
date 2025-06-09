using WorkHunter.Models.Dto.Users;
using WorkHunter.Models.Views.WorkHunters;

namespace WorkHunterHelper.Services;

public interface IWorkHunterService
{
    Task<string> GetToken(LoginDto dto);

    Task<List<WResponseView>> GetResponses(string accessToken);
}
