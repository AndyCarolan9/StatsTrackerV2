namespace StatsTrackerV2.Utilities
{
    public static class PlatformHelper
    {
        public static bool IsDesktop =>
            DeviceInfo.Current.Platform == DevicePlatform.WinUI ||
            DeviceInfo.Current.Platform == DevicePlatform.MacCatalyst;

        public static bool IsMobile =>
            DeviceInfo.Current.Platform == DevicePlatform.Android ||
            DeviceInfo.Current.Platform == DevicePlatform.iOS;
    }
}
