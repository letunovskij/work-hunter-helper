using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Common.BackgroundTasks;

public abstract class BaseBackgroundTask<T> : BackgroundService where T : notnull
{
    protected readonly IServiceProvider serviceProvider;

    protected readonly ILogger logger;

    public virtual int Delay { get; } = BackgroundConstants.DefaultTaskDelay;
    public abstract Func<T, Task> Action { get; }
    public abstract bool IsEnabled { get; }

    protected BaseBackgroundTask(IServiceProvider serviceProvider, ILogger logger)
    {
        this.serviceProvider = serviceProvider;
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (IsEnabled)
            {
                try
                {
                    using var scope = serviceProvider.CreateScope();
                    var service = ActivatorUtilities.GetServiceOrCreateInstance<T>(scope.ServiceProvider);
                    await Action(service);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occured in {BackgroundTask}", this.ToString());
                }
            }
            await Task.Delay(Delay, stoppingToken);
        }
    }
}