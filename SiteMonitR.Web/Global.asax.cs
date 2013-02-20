using Microsoft.AspNet.SignalR;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace SiteMonitR.Web
{
    public class Global : System.Web.HttpApplication
    {
        internal static IKernel _kernel;

        protected void Application_Start(object sender, EventArgs e)
        {
            _kernel = new StandardKernel();
            _kernel.Bind<ISiteUrlRepository>().To<TableStorageSiteUrlRepository>();
            _kernel.Bind<ISiteResultReceiver>().To<WindowsAzureQueueSiteResultReceiver>().InSingletonScope();
            _kernel.Bind<IStorageQueueConfiguration>().To<WebSiteQueueConfiguration>();

            var resolver = new NinjectDependencyResolver(_kernel);
            GlobalHost.DependencyResolver = resolver;
            RouteTable.Routes.MapHubs();

            _kernel.TryGet<ISiteResultReceiver>().StartWatching();
        }
    }

    public class NinjectDependencyResolver : DefaultDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {
            if (kernel == null)
                throw new ArgumentNullException("kernel");

            _kernel = kernel;
        }

        public override object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType) ?? base.GetService(serviceType);
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType).Concat(base.GetServices(serviceType));
        }
    }
}