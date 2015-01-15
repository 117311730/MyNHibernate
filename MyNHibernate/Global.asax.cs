using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MyNHibernate.Infrastructure;
using NHibernate;
using NHibernate.Cfg;

namespace MyNHibernate
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(MvcApplication));
        private static ISessionContainer _SessionContaioner;
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();
            Log.Info("Startup application.");

            NHibernateUtility Utility = new NHibernateUtility();
            Utility.Initialize();

            _SessionContaioner = new HttpContextSessionContainer(NHibernateUtility.GetSessionFactory());

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest()
        {
            _SessionContaioner.OpenSession();
        }

        protected void Application_EndRequest()
        {
            _SessionContaioner.CloseSession(); 
        }

        public static ISession GetCurrentSession()
        {
            return _SessionContaioner.Session;
        }
    }
}