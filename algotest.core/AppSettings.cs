namespace swg.mining.core
{
    public static class AppSettings
    {
        public static string Secret { get; set; }
        public static string ConnectionString { get; set; }
    }
    public class AppSettingModel
    {
        public string Secret { get; set; }
        public string ConnectionString { get; set; }
    }
}