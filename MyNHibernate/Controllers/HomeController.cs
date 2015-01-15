using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate;
using MyNHibernate.Models;
using MyNHibernate.Services;
namespace MyNHibernate.Controllers
{
    public class HomeController : Controller
    {
        private readonly CityService _CityService;

        public HomeController()
        {
            _CityService = new CityService();
        }
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult City()
        {
            var model = _CityService.GetAll();
            return View(model);
        }
    }
}
