using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyNHibernate.Infrastructure;
using NHibernate;

namespace MyNHibernate.Services
{
    public class BaseRepository
    {
        protected ISession Session;

        public BaseRepository()
        {
            //Session = new HttpContextSessionContainer(NHibernateUtility.GetSessionFactory()).Session;
            Session = MvcApplication.GetCurrentSession();
        }

        public IEnumerable<T> GetAll<T>() where T : class
        {
            return Session.QueryOver<T>().List<T>();
        }

        public T GetOne<T>(int id) where T : class
        {
            return Session.Get<T>(id);
        }

    }
}