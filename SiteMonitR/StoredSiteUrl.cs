using Microsoft.WindowsAzure.Storage.Table.DataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SiteMonitR
{
    public class StoredSiteUrl : TableServiceEntity
    {
        public StoredSiteUrl()
        {
            this.PartitionKey = "default";
            this.RowKey = Guid.NewGuid().ToString();
            this.Timestamp = DateTime.Now;
        }

        public string Url { get; set; }
    }
}
