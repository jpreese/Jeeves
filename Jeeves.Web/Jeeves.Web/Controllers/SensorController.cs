using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jeeves.Web.Controllers
{
    public class SensorController : Controller
    {
        public ActionResult Temperature(DateTime timestamp, int temp)
        {
            return View();
        }

    }
}
