using System;
using NHibernate;

namespace MyNHibernate.Infrastructure
{
    public interface ISessionContainer : IDisposable
    {
        ISession Session { get; set; }

        void OpenSession();

        void CloseSession();
    }
}