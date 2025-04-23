using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WorkHunter.BackgroundTasks;
using WorkHunterUtils.Abstractions.Currencies;
using WorkHunterUtils.Models.Options;

namespace WorkHunterUtils.BackgroundTasks.Tasks;

public sealed class GetCurrentCurrencyTask : BaseBackgroundTask<ICurrencyService>
{
    private readonly IOptionsMonitor<CurrencyOptions> taskOptions;

    public override int Delay => BackgroundConstants.DefaultTaskDelay;

    public override bool IsEnabled => taskOptions.CurrentValue.IsEnable;

    public override Func<ICurrencyService, Task> Action
        => (service) => service.GetCurrencies();

    public GetCurrentCurrencyTask(
        IOptionsMonitor<CurrencyOptions> taskOptions,
        IServiceProvider services,
        ILogger<GetCurrentCurrencyTask> logger) : base(services, logger)
    {
        this.taskOptions = taskOptions;
    }
}
