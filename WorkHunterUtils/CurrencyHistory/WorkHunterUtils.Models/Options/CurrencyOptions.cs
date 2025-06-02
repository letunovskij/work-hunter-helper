using Common.BackgroundTasks;

namespace WorkHunterUtils.Models.Options;

public sealed class CurrencyOptions : BaseBackgroundTaskOptions
{
    public string BaseUrl { get; set; }

    public double DiffThreshold { get; set; }
}
