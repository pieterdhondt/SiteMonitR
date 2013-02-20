using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace SiteMonitR
{
    public class WebSiteQueueConfiguration : IStorageQueueConfiguration
    {
        public WebSiteQueueConfiguration()
        {

        }

        public string GetConnectionString()
        {
            return ConfigurationManager.AppSettings["SiteMonitRConnectionString"];
        }

        public string GetIncomingQueueName()
        {
            return "sitestatusresults";
        }
    }
}
