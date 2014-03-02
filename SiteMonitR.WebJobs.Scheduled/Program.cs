using Microsoft.WindowsAzure.Jobs;
using SiteMonitR.Common;
using System;
using System.Collections.Generic;
using System.Net;

namespace SiteMonitR.WebJobs.Scheduled
{

    class Program
    {
        [NoAutomaticTrigger]
        public static void CheckSitesFunction(

            // the table containing the list of sites
            [Table(SiteMonitRConfiguration.TABLE_NAME_SITES)] 
                IDictionary<Tuple<string, string>, SiteRecord> siteRecords,

            // the queue that will receive site results
            [QueueOutput(SiteMonitRConfiguration.QUEUE_NAME_INCOMING_SITE_LOG)] 
                out IEnumerable<SiteResult> siteResults
            )
        {

            // create a new list of the result classes
            var resultList = new List<SiteResult>();

            foreach (var nv in siteRecords)
            {
                // create a new result for this site
                var siteResult = new SiteResult
                {
                    Uri = nv.Value.Uri,
                    Status = SiteMonitRConfiguration.DASHBOARD_SITE_CHECKING
                };

                // update the UX to let the user know we're checking the site
                SiteMonitRConfiguration.UpdateDashboard(siteResult);

                // check the site
                var request = (HttpWebRequest)HttpWebRequest.Create(siteResult.Uri);

                try
                {
                    // the site is up
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    siteResult.Status = SiteMonitRConfiguration.DASHBOARD_SITE_UP;
                }
                catch (Exception)
                {
                    // the site is down
                    siteResult.Status = SiteMonitRConfiguration.DASHBOARD_SITE_DOWN;
                }

                // add the result to the list
                resultList.Add(siteResult);

                // update the UX to let the user know we're done checking this site
                SiteMonitRConfiguration.UpdateDashboard(siteResult);
            }

            // set the output value, sending the messages into the queue individually
            siteResults = resultList;

        }

        static void Main(string[] args)
        {
            JobHost host = new JobHost();
            var methodInfo = typeof(Program).GetMethod("CheckSitesFunction");
            host.Call(methodInfo);
        }
    }


}
