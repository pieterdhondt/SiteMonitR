using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace SiteMonitR.Web.Hubs
{
    [HubName("SiteMonitR")]
    public class SiteMonitRNotificationHub : Hub
    {
        private ISiteUrlRepository _repository;
        private ISiteResultReceiver _siteResultReceiver;

        public SiteMonitRNotificationHub(ISiteUrlRepository repository,
            ISiteResultReceiver resultReceiver)
        {
            _repository = repository;
            _siteResultReceiver = resultReceiver;

            _siteResultReceiver.StatusUpdated += OnReceiveSiteResult;
        }

        int mtdCnt = 0;

        private void OnReceiveSiteResult(object sender, SiteResultEventArgs e)
        {
            mtdCnt += 1;

            Debug.WriteLine("{0} OnReceiveSiteResult called {1} times on hub {2}",
                base.Context.ConnectionId, 
                mtdCnt,
                base.Context.ConnectionId);

            Clients.Caller.siteStatusUpdated(e.Result);
        }

        public void AddSite(string url)
        {
            _repository.Add(url);
            Clients.All.siteAddedToGui(url);
        }

        public void RemoveSite(string url)
        {
            _repository.Remove(url);
            Clients.All.siteRemovedFromGui(url);
        }

        public void GetSiteList()
        {
            var urls = _repository.GetUrls();
            Clients.All.siteListObtained(urls);
        }

        public void CheckSite(string url)
        {
            Clients.All.checkingSite(url);
        }
    }
}
