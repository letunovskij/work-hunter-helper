using Cronos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Common.BackgroundTasks
{
    public abstract class BaseCronTask<TService, TOptions> : IHostedService, IDisposable where TService : notnull where TOptions : CronTaskOptions
    {
        private bool disposed = false;

        private System.Timers.Timer? timer;

        private readonly TimeZoneInfo timeZone = TimeZoneInfo.Local; 

        private readonly ILogger logger;

        private readonly IServiceProvider serviceProvider;

        private readonly IOptionsMonitor<TOptions> cronOptions;

        public abstract Func<TService, Task> Action { get; }

        protected BaseCronTask(
            IOptionsMonitor<TOptions> cronOptions,
            IServiceProvider serviceProvider,
            ILogger logger)
        {
            this.cronOptions = cronOptions; 
            this.serviceProvider = serviceProvider;
            this.logger = logger;
        }

        public virtual async Task StartAsync(CancellationToken cancellationToken) => await ScheduleJob(cancellationToken);

        protected virtual async Task ScheduleJob(CancellationToken cancellationToken)
        {
            var schedule = CronExpression.Parse(cronOptions.CurrentValue.Schedule);

            var next = schedule.GetNextOccurrence(DateTimeOffset.Now, timeZone);

            if (next.HasValue)
            {
                var delay = next.Value - DateTimeOffset.Now;
                if (delay.Microseconds <= 0)
                {
                    await ScheduleJob(cancellationToken);
                }

                if (timer != null) timer.Dispose();
                timer = new System.Timers.Timer();
                timer.Interval = delay.TotalMilliseconds;

                timer.Elapsed += async (sender, args) =>
                {
                    if (!cancellationToken.IsCancellationRequested)
                    {
                        await DoWork(cancellationToken);
                        await ScheduleJob(cancellationToken);
                    }
                };

                timer.Start();
            }
        }

        private async Task DoWork(CancellationToken cancellationToken)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<TService>();
                await Action(service);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "BaseCronTask Error has occured in {TypeName}", GetType().Name);
            }
        }
        
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    {
                        timer?.Dispose();
                    }

                    this.disposed = true;
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Stop();
            return Task.CompletedTask;
        }
    }
}
