using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyNHibernate.Infrastructure;

namespace MyNHibernate.Filters
{
    public class UnitOfWorkFilter : FilterAttribute, IActionFilter
    {
        private readonly ISessionContainer _sessionContainer;


        public UnitOfWorkFilter(ISessionContainer sessionContainer)
        {
            Contract.Requires(sessionContainer != null);

            _sessionContainer = sessionContainer;
        }


        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            _sessionContainer.CloseSession();
        }


        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _sessionContainer.OpenSession();
        }
    }
}