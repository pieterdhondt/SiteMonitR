// ---------------------------------------------------------------------------------- 
// Microsoft Developer & Platform Evangelism 
//  
// Copyright (c) Microsoft Corporation. All rights reserved. 
//  
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,  
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES  
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE. 
// ---------------------------------------------------------------------------------- 
// The example companies, organizations, products, domain names, 
// e-mail addresses, logos, people, places, and events depicted 
// herein are fictitious.  No association with any real company, 
// organization, product, domain name, email address, logo, person, 
// places, or events is intended or should be inferred. 
// ---------------------------------------------------------------------------------- 

using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteMonitR.Web.Hubs
{
    [HubName("SiteMonitR")]
    public class SiteMonitRNotificationHub : Hub
    {
        public void ServiceReady()
        {
            Clients.All.serviceIsUp();
        }

        public void ReceiveMonitorUpdate(dynamic monitorUpdate)
        {
            Clients.All.siteStatusUpdated(monitorUpdate);
        }

        public void AddSiteToGui(string url)
        {
            Clients.All.siteAddedToGui(url);
        }

        public void RemoveSiteFromGui(string url)
        {
            Clients.All.siteRemovedFromGui(url);
        }

        public void AddSite(string url)
        {
            Clients.All.siteAddedToStorage(url);
        }

        public void RemoveSite(string url)
        {
            Clients.All.siteRemovedFromStorage(url);
        }

        public void GetSiteList()
        {
            Clients.All.siteListRequested();
        }

        public void ListOfSitesObtained(List<string> urls)
        {
            Clients.All.siteListObtained(urls);
        }

        public void CheckSite(string url)
        {
            Clients.All.checkingSite(url);
        }
    }
}
