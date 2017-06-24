using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jeeves.Web.Models;

namespace Jeeves.Web.Data
{
    public interface ITemperatureRepository
    {
        IEnumerable<Temperature> GetLatestTemperatureReadings(int numberOfReadings = 5);
    }
}