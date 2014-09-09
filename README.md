# SiteMonitR Sample #

The SiteMonitR Cloud Service pings all of the sites in a list of sites stored in Microsoft Azure Storage. As each site's status is obtained a message is sent to a storage queue. A WebJob running in a Web Site picks up the results of each site's status check and saves the log entry to a storage table. Hosted in a Web Site in Azure, a Web API controller receives messages from the WebJobs when sites are pinged. The Web API controller then sends updates to the SiteMonitR dashboard via a SignalR Hub. The result is a web-based web site monitoring tool. 

### Prerequisites

* [Visual Studio 2013](http://www.microsoft.com/visualstudio/en-us/products) 
* [Windows Azure SDK for .NET 2.2](http://www.windowsazure.com/en-us/develop/net/)

### Setting up the Sample

The instructions below will walk you through the process of setting up SiteMonitR both locally on your development workstation and for getting it running live in Microsoft Azure. 

1. Download the code
1. Open the solution in Visual Studio and compile it. NuGet package restore should pull down all of the required NuGet packages automatically
1. Go to the **bin/Debug** folder of the SiteMonitR.WebJobs.EventDriven project. Zip all of the files (excluding .PDB and .XML files or any file with the term **vshost**). Rename the zip file to **EventDriven.zip** and copy it to your desktop.
1. Go to the **bin/Debug** folder of the SiteMonitR.WebJobs.Scheduled project. Zip all of the files (excluding .PDB and .XML files or any file with the term **vshost**). Rename the zip file to **Scheduled.zip** and copy it to your desktop.
1. Publish the SiteMonitR.Web web project into a new Windows Azure Web Site
1. If you don't have any Storage accounts in your Azure subscription, create one in the Azure portal.
1. Copy the storage account's name and primary (or secondary) keys, and build a string representing the storage account connection string. The format of this string looks like this:

    DefaultEndpointsProtocol=https;AccountName=[YOUR ACCOUNT NAME];AccountKey=[YOUR ACCOUNT KEY]

1. Go to the Web Site's **Configure** tab in the Management Portal
1. Create a new Connection String for the web site **using the Azure Management Portal** *(just setting the value in your Web.config file won't work, you need to set this using the portal)* you just deployed named **AzureWebJobsDashboard** and paste the connection string as the value of the Connection String.
1. Create a new Connection String for the web site **using the Azure Management Portal** *(just setting the value in your Web.config file won't work, you need to set this using the portal)* you just deployed named **AzureWebJobsStorage** and paste the connection string as the value of the Connection String.
1. Create a new App Setting for the web site **using the Azure Management Portal**. The name of the App Setting should be set to **SiteMonitR.DashboardUrl** and the value should be your web site's root URL (i.e., http://sitemonitr.azurewebsites.net)
1. Go to the **WebJobs** tab for the Web Site in the Management Portal
1. Click the **Add a Job** link
1. Name the job "Event Driven" and upload the **EventDriven.zip** file from your desktop. Select **Run Continuously** from the **How to Run** drop-down menu.
1. Go to the **WebJobs** tab for the Web Site in the Management Portal
1. Click the **Add** button at the bottom of the Management Portal
1. Name the job **Scheduled** and upload the **Scheduled.zip** file from your desktop. Select **Run on a Schedule** from the **How to Run** drop-down menu.
1. Set the schedule for the Scheduled WebJob per your liking. A suggestion is to set it to run every 15 minutes, but you can specify it for however often you prefer for your sites to be pinged. 
1. Restart the web site
1. Click the **Browse** button from within the Management Portal to browse the site. 
1. Add sites you wish to monitor.
1. If you'd like to force the WebJob to ping the sites you add from the SiteMonitR Web Dashboard, select the **Scheduled** WebJob and click the **Run** button at the bottom of the Management Portal.