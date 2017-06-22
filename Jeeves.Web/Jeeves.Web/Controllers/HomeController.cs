using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jeeves.Web.Data;
using Jeeves.Web.Models;

namespace Jeeves.Web.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Returns the Home view.
        /// </summary>
        /// <returns>The Home view.</returns>
        public ActionResult Index()
        {
            INotificationRepository _notificationRepository = new NotificationRepository();

            if (Request.IsAuthenticated)
            {
                IEnumerable<Temperature> temps;
                using (var db = new DataContext())
                {
                    temps = db.Temperatures.Take(5).OrderByDescending(m => m.ReadDate).ToList();
                }

                ViewBag.Temperatures = temps;
                ViewBag.Notification = _notificationRepository.GetNotification();

                return View("_Dashboard", temps);
            }
            else
            {
                return View("_Login");
            }
        }
    }
}
