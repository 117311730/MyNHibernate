using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;

namespace MyNHibernate.Infrastructure
{
    public class HttpContextSessionContainer : ISessionContainer
    {
        private readonly ISessionFactory _sessionFactory;

        public HttpContextSessionContainer(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void Dispose()
        {
            if (Session.IsConnected)
                Session.Close();

            Session.Dispose();

            HttpContext.Current.Items.Remove("NhibSession");
        }

        public ISession Session
        {
            get
            {
                return HttpContext.Current.Items["NhibSession"] as ISession;
            }
            set
            {
                HttpContext.Current.Items.Add("NhibSession", value);
            }
        }

        public void OpenSession()
        {
            Session = _sessionFactory.OpenSession();
        }

        public void CloseSession()
        {
            this.Dispose();
        }

    }
}