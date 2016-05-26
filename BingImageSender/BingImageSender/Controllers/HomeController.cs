using BingImageSender.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BingImageSender.Controllers
{
    public class HomeController : Controller
    {
        [HttpPost]
        public ActionResult Send(string email)
        {

            EmailScheduler.Start(email);

            return View("Index");
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}