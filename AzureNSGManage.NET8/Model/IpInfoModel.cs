using System;
using System.Collections.Generic;
using System.Text;

namespace AzureNSGManage.NET8.Model
{
    public class IpInfoModel
    {
        public int Rs { get; set; }            // "rs" 对应的字段
        public int Code { get; set; }          // "code" 对应的字段
        public string Address { get; set; }    // "address" 对应的字段
        public string Ip { get; set; }         // "ip" 对应的字段
        public int IsDomain { get; set; }      // "isDomain" 对应的字段
    }
}
