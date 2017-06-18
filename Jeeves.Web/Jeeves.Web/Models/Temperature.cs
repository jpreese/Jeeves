using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jeeves.Web.Models
{
    public class Temperature
    {
        public int TemperatureID { get; set; }
        public DateTime ReadDate { get; set; }
        public int Reading { get; set; }
    }
}
