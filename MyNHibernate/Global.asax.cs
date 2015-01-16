using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MyNHibernate.Infrastructure;
using NHibernate;
using NHibernate.Cfg;
using Quartz;
using Quartz.Impl;

namespace MyNHibernate
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(MvcApplication));
        private static ISessionContainer _SessionContaioner;
        private IndexScheduler Sched;

        public MvcApplication()
        {

        }

        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();
            Log.Info("Startup application.");

            NHibernateUtility Utility = new NHibernateUtility();
            Utility.Initialize();

            _SessionContaioner = new HttpContextSessionContainer(NHibernateUtility.GetSessionFactory());

            Sched = new IndexScheduler();
            Sched.Start();

            RegisterRoutes();
        }

        protected void Application_BeginRequest()
        {
            _SessionContaioner.OpenSession();
        }

        protected void Application_EndRequest()
        {
            _SessionContaioner.CloseSession();
        }

        protected void Application_End()
        {
            Sched.Close();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();

            //log error
            LogException(exception);
        }

        public static ISession GetCurrentSession()
        {
            return _SessionContaioner.Session;
        }

        protected void LogException(Exception exc)
        {
            if (exc == null)
                return;

            //ignore 404 HTTP errors
            var httpException = exc as HttpException;
            if (httpException != null && httpException.GetHttpCode() == 404)
                return;

            try
            {
                //log
                Log.Error(exc.Message, exc);
            }
            catch (Exception)
            {
                //don't throw new exception if occurs
            }
        }

        private void RegisterRoutes()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }
    }
}