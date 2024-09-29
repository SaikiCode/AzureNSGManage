using Microsoft.Extensions.Configuration;

namespace AzureNSGManage.NET8.Config
{
    public class AppConfig
    {
        private static readonly AppConfig _instance;

        static AppConfig()
        {
            if (_instance == null)
            {
                var appConfig = new ConfigurationBuilder()
                      .AddJsonFile("appsettings.json")
                      .Build()
                      .Get<AppConfig>();
                _instance = appConfig;
            }
        }

        public static AppConfig Instance
        {
            get
            {
                return _instance;
            }
        }
        public AzureApp AzureApp { get; set; }

        public string NSGID { get; set; }

        public string NSGRuleName { get; set; }

        public bool UseAzure { get; set; }

    }

    public class AzureApp
    {
        public string AppId { get; set; }

        public string Appkey { get; set; }

        public string TenantId { get; set; }

        public string SubscriptionId { get; set; }
    }
}
