using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SiteMonitR
{
    [Serializable]
    public class SiteResult
    {
        public string Url { get; set; }
        public string Status { get; set; }
    }
}
