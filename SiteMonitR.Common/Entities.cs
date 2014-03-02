using System;

namespace SiteMonitR.Common
{
    public class SiteRecord
    {
        public string Uri { get; set; }
    }

    public class SiteResult
    {
        public string Uri { get; set; }
        public string Status { get; set; }
    }

    public class RefreshDashboardParameters
    {
        public SiteResult SiteResult { get; set; }
    }
}