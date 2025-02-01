using Mapster;
using WorkHunter.Models.Dto.WorkHunters;
using WorkHunter.Models.Entities.WorkHunters;
using WorkHunter.Models.Views.WorkHunters;

namespace WorkHunter.Api.Config.MappingProfiles;

public sealed class WResponseMappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<WResponse, WResponseView>();

        config.NewConfig<WResponseUpdateDto, WResponse>();

        config.NewConfig<WResponseCreateDto, WResponse>()
              .Inherits<WResponseUpdateDto, WResponse>()
              .Map(x => x.CreatedAt, src => DateTime.UtcNow);
    }
}
