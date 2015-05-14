using Microsoft.Azure.WebJobs;

namespace SiteMonitR.WebJobs.EventDriven
{
    class Program
    {
        static void Main(string[] args)
        {
            JobHost host = new JobHost();
            host.RunAndBlock();
        }
    }
}
