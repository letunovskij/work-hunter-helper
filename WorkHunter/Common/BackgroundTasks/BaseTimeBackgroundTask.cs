using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Common.BackgroundTasks;

public abstract class BaseTimeBackgroundTask<TService, TOptions> : BaseBackgroundTask<TService>
    where TService : notnull
    where TOptions : notnull, BaseTimeBackgroundTaskOptions
{
    public IOptionsMonitor<TOptions> Options { get; set; }

    public override bool IsEnabled => Options.CurrentValue.IsEnable;

    protected BaseTimeBackgroundTask(IServiceProvider serviceProvider, ILogger logger) : base(serviceProvider, logger)
    {
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (IsEnabled)
            {
                DateTime dateTime = DateTime.Now;
                var daysOfWeekForSend = Options.CurrentValue.DaysOfWeek;
                var timeToStart = Options.CurrentValue.TimeToStart;
                var timeToEnd = Options.CurrentValue.TimeToEnd;
                var day = (int)dateTime.DayOfWeek;
                if ((daysOfWeekForSend == null || daysOfWeekForSend.Contains(day)) && dateTime.Hour >= timeToStart && dateTime.Hour < timeToEnd)
                {
                    try
                    {
                        using var scope = serviceProvider.CreateScope();
                        var service = ActivatorUtilities.GetServiceOrCreateInstance<TService>(scope.ServiceProvider);
                        await Action(service);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occured in {BackgroundTask}", this.ToString());
                    }
                }
            }
            await Task.Delay(Delay, stoppingToken);
        }
    }
}