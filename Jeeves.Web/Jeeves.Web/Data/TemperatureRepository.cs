using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jeeves.Web.Models;

namespace Jeeves.Web.Data
{
    public class TemperatureRepository : ITemperatureRepository
    {
        /// <summary>
        /// Gets the latest temperature readings.
        /// </summary>
        /// <param name="numberOfReadings">The number of temperature readings to return.</param>
        /// <returns>A list of Temperatures objects.</returns>
        public IEnumerable<Temperature> GetLatestTemperatureReadings(int numberOfReadings = 5)
        {
            using(var db = new DataContext())
            {
                return db.Temperatures.OrderByDescending(t => t.ReadDate).Take(numberOfReadings).ToList();
            }
        }
    }
}