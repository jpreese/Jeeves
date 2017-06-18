using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jeeves.Web.Data;

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
            return View();
        }
    }
}
