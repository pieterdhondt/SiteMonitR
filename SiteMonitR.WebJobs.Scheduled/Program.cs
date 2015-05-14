﻿using Microsoft.Azure.WebJobs;

namespace SiteMonitR.WebJobs.Scheduled
{

    class Program
    {
        static void Main(string[] args)
        {
            JobHost host = new JobHost();
            var methodInfo = typeof(Functions).GetMethod("CheckSitesFunction");
            host.Call(methodInfo);
        }
    }
}
