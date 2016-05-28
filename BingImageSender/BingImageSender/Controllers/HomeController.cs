using BingImageSender.Jobs;
using BingImageSender.Models;
using System.Web.Mvc;

namespace BingImageSender.Controllers
{
    public class HomeController : Controller
    {
        public SubscribeContext _context = new SubscribeContext();
        [HttpPost]
        public ActionResult Send(string email)
        {
            if (_context.Subscribers.Find(email) != null && _context.Subscribers.Find(email).isSubscribed)
            {
                return View();
            }
            _context.Subscribers.Add(new Subscriber() { SubscriberId = email, isSubscribed = true });
            EmailScheduler.Start(email);
            _context.SaveChanges();

            return View("SuccessSubscribe");
        }

        public ActionResult Unsubscribe(string email)
        {
            EmailScheduler.Unsubscribe(email);

            _context.Subscribers.Remove(_context.Subscribers.Find(email));
            _context.SaveChanges();
            return View();
        }
        
        public ActionResult SuccessSubscribe()
        {
            return View();
        }

        public ActionResult Index()
        {
            EmailSender.DownloadImage();
            return View();
        }

    }
}