using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SiteMonitR
{
    public class TableStorageSiteUrlRepository : ISiteUrlRepository
    {
        private string _connectionStringName = "SiteMonitRConnectionString";
        private string _tableName = "sitemonitrurls";
        private CloudStorageAccount _storageAccount;
        private CloudTableClient _tableClient;
        private TableServiceContext _tableContext;

        public TableStorageSiteUrlRepository(IStorageQueueConfiguration configuration)
        {
            _storageAccount = CloudStorageAccount.Parse(configuration.GetConnectionString());

            _tableClient = _storageAccount.CreateCloudTableClient();
            _tableClient.GetTableReference(_tableName);
            _tableClient.GetTableReference(_tableName).CreateIfNotExists();

            _tableContext = _tableClient.GetTableServiceContext();
        }

        public List<string> GetUrls()
        {
            var r = _tableContext.CreateQuery<StoredSiteUrl>(_tableName);
            return r.ToList().Select(x => x.Url).ToList();
        }

        public void Add(string url)
        {
            _tableContext.AddObject(_tableName, new StoredSiteUrl { Url = url });
            _tableContext.SaveChanges();
        }

        public void Remove(string url)
        {
            var o = _tableContext.CreateQuery<StoredSiteUrl>(_tableName).ToList().First(x => x.Url == url);
            _tableContext.DeleteObject(o);
            _tableContext.SaveChanges();
        }
    }
}
