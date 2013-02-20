using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;

namespace SiteMonitR
{
    public class WindowsAzureQueueSiteResultReceiver
        : ISiteResultReceiver, IDisposable
    {
        CloudQueueClient _client;
        CloudQueue _incomingQueue;
        Thread _thread;
        IStorageQueueConfiguration _configuration;

        public event EventHandler<SiteResultEventArgs> StatusUpdated;

        public WindowsAzureQueueSiteResultReceiver(IStorageQueueConfiguration configuration)
        {
            _configuration = configuration;

            var connectionString = _configuration.GetConnectionString();
            var queueToReceiveFrom = _configuration.GetIncomingQueueName();

            _client = CloudStorageAccount.Parse(connectionString).CreateCloudQueueClient();

            if (!string.IsNullOrEmpty(queueToReceiveFrom))
            {
                _incomingQueue = _client.GetQueueReference(queueToReceiveFrom);
                _incomingQueue.CreateIfNotExists();
            }
        }

        public void StartWatching()
        {
            _thread = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    var msg = _incomingQueue.GetMessage();
                    if (msg != null)
                    {
                        var json = msg.AsString;
                        var siteResult = JsonConvert.DeserializeObject<SiteResult>(json);

                        if (siteResult != null)
                        {
                            if (this.StatusUpdated != null)
                            {
                                this.StatusUpdated(this, new SiteResultEventArgs(siteResult));
                                _incomingQueue.DeleteMessage(msg);
                            }
                        }
                    }
                    else
                    {
                        Thread.Sleep(1000);
                    }
                }
            }));

            _thread.Start();
        }

        public void Dispose()
        {
            if (_thread != null)
                _thread.Abort();
        }
    }
}
