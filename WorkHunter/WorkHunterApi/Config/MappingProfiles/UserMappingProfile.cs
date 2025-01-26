using Mapster;
using WorkHunter.Models.Entities;
using WorkHunter.Models.Views.Users;

namespace WorkHunter.Api.Config.MappingProfiles;

public class UserMappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, UserBaseView>();

        config.NewConfig<User, UserView>()
              .Inherits<User, UserBaseView>()
              .Map(x => x.Roles, src => src.UserRoles.Select(u => u.Role.Name).ToList());
    }
}
