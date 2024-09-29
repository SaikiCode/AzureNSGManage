using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AzureNSGManage.NET8.Model;
using Newtonsoft.Json;

namespace AzureNSGManage
{
    public class IpHelper
    {
        public static async Task<IpQueryModel> GetIpInfo()
        {
            string url = $"http://ip-api.com/json/";
            var model = new IpQueryModel();
            // 创建 HttpClientHandler 并禁用代理
            var handler = new HttpClientHandler
            {
                UseProxy = false
            };
            using (HttpClient client = new HttpClient(handler))
            {
                var responseStr = await client.GetStringAsync(url);
                model = JsonConvert.DeserializeObject<IpQueryModel>(responseStr);
            }
            return model;
        }

        public static string GetIPData(string token, string ip = null, string datatype = "txt")
        {
            string url = string.Format("https://api.ip138.com/ip/?ip={0}&datatype={1}&token={2}", ip, datatype, token);
            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                return client.DownloadString(url);
            }
        }

        public static async Task<PCOlineIpQueryModel> GetIpInfoByPCOnline()
        {
            var model = new PCOlineIpQueryModel();
            // 创建 HttpClientHandler 并禁用代理
            var handler = new HttpClientHandler
            {
                UseProxy = false
            };
            using (HttpClient client = new HttpClient(handler))
            {
                var responseStr = await client.GetStringAsync("https://whois.pconline.com.cn/ipJson.jsp?json=true&ip=");
                model = JsonConvert.DeserializeObject<PCOlineIpQueryModel>(responseStr);
            }
            return model;
        }

        public static async Task<IpInfoModel> GetIpAddress()
        {
            string url = $"https://ip.cn/api/index?ip=&type=0";
            var model = new IpInfoModel();
            // 创建 HttpClientHandler 并禁用代理
            var handler = new HttpClientHandler
            {
                UseProxy = false
            };
            using (HttpClient client = new HttpClient(handler))
            {
                var responseStr = await client.GetStringAsync(url);
                model = JsonConvert.DeserializeObject<IpInfoModel>(responseStr);
            }
            return model;
        }
    }
}
