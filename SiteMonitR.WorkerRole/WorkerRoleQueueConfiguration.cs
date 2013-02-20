using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SiteMonitR.WorkerRole
{
    public class WorkerRoleQueueConfiguration
        : IStorageQueueConfiguration
    {
        public WorkerRoleQueueConfiguration()
        {

        }

        public string GetConnectionString()
        {
            return RoleEnvironment.GetConfigurationSettingValue("SiteMonitRConnectionString");
        }

        public string GetIncomingQueueName()
        {
            return "sitestatusresults";
        }
    }
}
