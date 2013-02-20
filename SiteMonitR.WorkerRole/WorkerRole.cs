using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using SiteMonitR.WorkerRole.Properties;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace SiteMonitR.WorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        CloudStorageAccount _account;
        CloudQueueClient _queueClient;
        CloudQueue _queue;

        public override void Run()
        {
            Trace.WriteLine("SiteMonitR.WorkerRole entry point called", "Information");

            while (true)
            {
                var sites = new TableStorageSiteUrlRepository(
                    new WorkerRoleQueueConfiguration()
                    ).GetUrls();

                foreach (var url in sites)
                {
                    var result = new SiteResult { Url = url, Status = "Checking" };
                    var json = JsonConvert.SerializeObject(result);

                    _queue.AddMessage(new CloudQueueMessage(json));

                    try
                    {
                        new WebClient().DownloadString(url);
                        result = new SiteResult { Url = url, Status = "Up" };
                    }
                    catch
                    {
                        result = new SiteResult { Url = url, Status = "Down" };
                    }

                    json = JsonConvert.SerializeObject(result);
                    _queue.AddMessage(new CloudQueueMessage(json));
                }

                Thread.Sleep(Settings.Default.PingTimeout);
            }
        }

        public override bool OnStart()
        {
            _account = CloudStorageAccount.Parse(
                    RoleEnvironment.GetConfigurationSettingValue("SiteMonitRConnectionString")
                    );

            _queueClient = _account.CreateCloudQueueClient();
            _queue = _queueClient.GetQueueReference(new WebSiteQueueConfiguration().GetIncomingQueueName());
            _queue.CreateIfNotExists();

            return base.OnStart();
        }
    }
}
