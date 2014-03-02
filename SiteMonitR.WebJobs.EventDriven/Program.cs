using Microsoft.WindowsAzure.Jobs;
using SiteMonitR.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteMonitR.WebJobs.EventDriven
{
    class Program
    {

        public static void AddSite(

            // the incoming queue
            [QueueInput(SiteMonitRConfiguration.QUEUE_NAME_NEW_SITE)] string url,

            // the table into which sites should be saved
            [Table(SiteMonitRConfiguration.TABLE_NAME_SITES)] 
                IDictionary<Tuple<string, string>, SiteRecord> siteRecords
            )
        {
            var cleansedUrl = SiteMonitRConfiguration.CleanUrlForRowKey(url);

            var key = new Tuple<string, string>(
                SiteMonitRConfiguration.GetPartitionKey(), cleansedUrl);

            if (!siteRecords.ContainsKey(key))
                siteRecords.Add(key, new SiteRecord { Uri = url });
        }

        public static void DeleteSite(

            // the incoming queue
            [QueueInput(SiteMonitRConfiguration.QUEUE_NAME_DELETE_SITE)] string url,

            // the site list table from which data should be deleted
            [Table(SiteMonitRConfiguration.TABLE_NAME_SITES)] 
                IDictionary<Tuple<string, string>, SiteRecord> siteRecords,

            // the site log table from which data should be deleted
            [Table(SiteMonitRConfiguration.TABLE_NAME_SITE_LOGS)] 
                IDictionary<Tuple<string, string>, SiteResult> siteResults
            )
        {
            var cleansedUrl = SiteMonitRConfiguration.CleanUrlForRowKey(url);

            var key = new Tuple<string, string>(
                SiteMonitRConfiguration.GetPartitionKey(), cleansedUrl);

            // delete the site record
            if (siteRecords.ContainsKey(key))
            {
                siteRecords.Remove(key);
            }

            // delete all the site's logs
            foreach (var siteResult in siteResults)
            {
                if(siteResult.Key.Item1 == cleansedUrl)
                {
                    siteResults.Remove(siteResult.Key);
                }
            }
        }


        public static void SaveSiteLogEntry(

            // the incoming queue
            [QueueInput(SiteMonitRConfiguration.QUEUE_NAME_INCOMING_SITE_LOG)] 
                SiteResult siteResult,

            // the site log table, into which data will be saved
            [Table(SiteMonitRConfiguration.TABLE_NAME_SITE_LOGS)] 
                IDictionary<Tuple<string, string>, SiteResult> siteResults
            )
        {
            var tuple = new Tuple<string, string>(
                SiteMonitRConfiguration.CleanUrlForRowKey(siteResult.Uri),
                Path.GetFileNameWithoutExtension(Path.GetRandomFileName())
                );

            siteResults.Add(new KeyValuePair<Tuple<string, string>, SiteResult>(
                tuple, siteResult));
        }


        static void Main(string[] args)
        {
            JobHost host = new JobHost();
            host.RunAndBlock();
        }



    }
}
