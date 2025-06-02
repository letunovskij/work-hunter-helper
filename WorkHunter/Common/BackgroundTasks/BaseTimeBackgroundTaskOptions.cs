namespace Common.BackgroundTasks
{
    public class BaseTimeBackgroundTaskOptions : BaseBackgroundTaskOptions
    {
        public List<int>? DaysOfWeek { get; set; } 

        public required int TimeToStart { get;set; }

        public required int TimeToEnd { get; set; }
    }
}
