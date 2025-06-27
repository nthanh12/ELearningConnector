namespace ElearningConnector.BackgroundServices
{
    public class NhanVienChangeCheckerOptions
    {
        public int CheckIntervalMinutes { get; set; } = 5;
        public string CheckApiUrl { get; set; }
        public string NotifyApiUrl { get; set; }
        public string BackgroundServiceApiKey { get; set; }
    }
} 