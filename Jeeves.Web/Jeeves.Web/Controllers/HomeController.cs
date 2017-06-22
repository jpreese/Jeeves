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
            ITemperatureRepository _temperatureRepository = new TemperatureRepository();
            INotificationRepository _notificationRepository = new NotificationRepository(_temperatureRepository);

            if (Request.IsAuthenticated)
            {
                ViewBag.Temperatures = _temperatureRepository.GetLatestTemperatureReadings();
                ViewBag.Notification = _notificationRepository.GetNotification();

                return View("_Dashboard");
            }
            else
            {
                return View("_Login");
            }
        }
    }
}
