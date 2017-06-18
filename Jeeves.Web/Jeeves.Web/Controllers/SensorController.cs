using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jeeves.Web.Data;
using Jeeves.Web.Models;

namespace Jeeves.Web.Controllers
{
    public class SensorController : Controller
    {
        public Temperature LogTemperature(Temperature temp)
        {
            using (var db = new DataContext())
            {
                db.Temperatures.Add(temp);
                db.SaveChanges();
            }

            return temp;
        }

    }
}
