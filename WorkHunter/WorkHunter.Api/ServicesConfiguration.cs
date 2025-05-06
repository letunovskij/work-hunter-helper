using Abstractions.Users;
using Common.Exceptions;
using FluentValidation;
using System.Security.Principal;
using WorkHunter.Abstractions.Exports;
using WorkHunter.Abstractions.Imports;
using WorkHunter.Abstractions.WorkHunters;
using WorkHunter.Models.Config;
using WorkHunter.Models.Dto.Users.Validators;
using WorkHunter.Services.Exports;
using WorkHunter.Services.Imports;
using WorkHunter.Services.WorkHunters;

namespace WorkHunter.Api;

public static class ServicesConfiguration
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddOptions<AuthOptions>()
                .Bind(config.GetSection("AuthOptions"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

        services.AddHttpContextAccessor()
                .AddScoped<IPrincipal>(x => x.GetService<IHttpContextAccessor>()?.HttpContext?.User 
                                         ?? throw new BusinessErrorException("IHttpContextAccessor не сконфигурирован!"));

        services.AddValidatorsFromAssemblyContaining<LoginDtoValidator>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IWResponseService, WResponseService>();
        services.AddScoped<IWResponseImportService, WResponseImportService>();
        services.AddScoped<IWResponsesExportService, WResponsesExportService>();        

        return services;
    }
}
