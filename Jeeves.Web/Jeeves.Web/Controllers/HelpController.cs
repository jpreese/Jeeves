using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jeeves.Web.Controllers
{
    public class HelpController : Controller
    {
        public ActionResult Index()
        {
            return View("_Help");
        }

    }
}
