# SiteMonitR Sample #

The SiteMonitR Cloud Service pings all of the sites in a list of sites stored in Azure Storage. As each site's status is obtained a message is sent to a storage queue. A WebJob running in a Web Site picks up the results of each site's status check and saves the log entry to a storage table. Hosted in a Web Site in Azure, a Web API controller receives messages from the WebJobs when sites are pinged. The Web API controller then sends updates to the SiteMonitR dashboard via a SignalR Hub. The result is a web-based web site monitoring tool. 

### Prerequisites

* [Visual Studio 2013](http://www.microsoft.com/visualstudio/en-us/products) 
* [Windows Azure SDK for .NET 2.2](http://www.windowsazure.com/en-us/develop/net/)

### Setting up the Sample

TBD