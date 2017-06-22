using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jeeves.Web.Models;

namespace Jeeves.Web.Data
{
    public class TemperatureRepository : ITemperatureRepository
    {
        public IEnumerable<Temperature> GetLatestTemperatureReadings()
        {
            using(var db = new DataContext())
            {
                return db.Temperatures.OrderByDescending(t => t.ReadDate).Take(5).ToList();
            }
        }
    }
}