﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jeeves.Web.Models;

namespace Jeeves.Web.Data
{
    public interface IUserValidationProvider
    {
        bool VerifyUserCredentials(User userModel);
    }
}
