using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Network;
using Azure.ResourceManager.Network.Models;
using AzureNSGManage.NET8.Config;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace AzureNSGManage.NET8
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Environment.CurrentDirectory = AppContext.BaseDirectory;

            // 创建配置对象，读取 serilog.json
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("serilog.json")
                .Build();

            // 使用 Serilog 配置
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
            var res = IpHelper.GetIpInfo().Result;
            Log.Information($"本机公网IP为:{res.query}");


            //添加Azure入站白名单
            UpdateAzurePortAddress(res.query).Wait();

            //计划任务启动直接退出程序
            if (args != null && args.Length > 0)
            {
                return;
            }
            Console.WriteLine("按任意键退出程序");
            Console.ReadKey();
        }

        public static async Task UpdateAzurePortAddress(string ipAddress)
        {
            var credentials = new ClientSecretCredential(AppConfig.Instance.AzureApp.TenantId, AppConfig.Instance.AzureApp.AppId, AppConfig.Instance.AzureApp.Appkey);

            ArmClient client = new ArmClient(credentials);

            // 获取 NSG
            var networkSecurityGroup = await client.GetNetworkSecurityGroupResource(new Azure.Core.ResourceIdentifier(AppConfig.Instance.NSGID)).GetAsync();
            // 获取SecurityRule
            var rule = await networkSecurityGroup.Value.GetSecurityRuleAsync(AppConfig.Instance.NSGRuleName);
            if (rule != null)
            {
                if (rule.Value.Data.SourceAddressPrefixes.Contains(ipAddress))
                {
                    Log.Information($"本IP已在白名单上,无须添加");
                }
                else
                {
                    rule.Value.Data.SourceAddressPrefixes.Add(ipAddress);

                    var response = await rule.Value.UpdateAsync(Azure.WaitUntil.Completed, rule.Value.Data);

                    if (response != null && response.Value.Data.SourceAddressPrefixes.Contains(ipAddress))
                    {
                        Log.Information($"已成功添加IP:{ipAddress}到白名单");
                    }
                    else
                    {
                        Log.Information("添加失败,请在Portal上手动添加");
                    }
                }
            }
        }
    }
}
