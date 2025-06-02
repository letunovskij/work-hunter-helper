using Mapster;
using WorkHunter.Models.Entities.Notifications;
using WorkHunter.Models.Entities.Users;
using WorkHunter.Models.Views.Notifications;
using WorkHunter.Models.Views.Users;

namespace WorkHunter.Api.Config.MappingProfiles;

public sealed class TaskMappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UserTask, UserTaskView>();

        config.NewConfig<User, UserView>()
              .Inherits<User, UserBaseView>()
              .Map(x => x.Roles, src => src.UserRoles == null ? null : src.UserRoles.Select(u => u.Role!.Name));
    }
}
