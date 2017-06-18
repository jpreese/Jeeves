﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Jeeves.Web.Models;

namespace Jeeves.Web.Data
{
    public class TemperatureContext : DbContext
    {
        DbSet<Temperature> Temperatures { get; set; }
    }
}
