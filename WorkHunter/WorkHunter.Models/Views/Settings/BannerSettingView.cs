namespace WorkHunter.Models.Views.Settings
{
    public sealed class BannerSettingView
    {
        public required string Description { get; set; }

        public bool IsActive { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
