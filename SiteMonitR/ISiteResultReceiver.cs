using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SiteMonitR
{
    public interface ISiteResultReceiver
    {
        void StartWatching();
        event EventHandler<SiteResultEventArgs> StatusUpdated;
    }

    public class SiteResultEventArgs : EventArgs
    {
        public SiteResultEventArgs(SiteResult result)
        {
            Result = result;
        }

        public SiteResult Result { get; private set; }
    }
}
