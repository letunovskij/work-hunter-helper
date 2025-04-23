using Polly;
using Prometheus;
using WorkHunterUtils.Abstractions.Currencies;
using WorkHunterUtils.Abstractions.HttpClients;
using WorkHunterUtils.BackgroundTasks.Tasks;
using WorkHunterUtils.Models.Options;
using WorkHunterUtils.Services.Currencies;
using WorkHunterUtils.Services.HttpClients;

namespace CurrencyHistory.WebHost;

internal static class ServiceConfiguration
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        RegisterOptions(services, configuration);
        RegisterServices(services, configuration);
        RegisterHttpServices(services, configuration);
        RegisterTaskServices(services, configuration);

        services.UseHttpClientMetrics();
        return services;
    }

    private static void RegisterHttpServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<ICurrencyClient, CurrencyClient>(x =>
                 {
                     var baseUrl = configuration.GetValue<string>("BackgroundTaskOptions:CurrencyOptions:BaseUrl");
                     if (!string.IsNullOrEmpty(baseUrl))
                         x.BaseAddress = new Uri(baseUrl);
                     else
                         x.BaseAddress = null;
                 })
                 .ConfigurePrimaryHttpMessageHandler(x => new SocketsHttpHandler())
                 .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, (i) => TimeSpan.FromSeconds(2 * i)));
    }

    private static void RegisterTaskServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHostedService<GetCurrentCurrencyTask>();
    }

    private static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICurrencyService, CurrencyService>();
    }

    public static void RegisterOptions(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptionsWithValidateOnStart<CurrencyOptions>()
                .Bind(configuration.GetSection("BackgroundTaskOptions:CurrencyOptions"))
                .ValidateDataAnnotations();
    }
}
