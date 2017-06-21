﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jeeves.Web.Models;
using Jeeves.Web.Data;
using System.Web.Security;

namespace Jeeves.Web.Controllers
{
    public class LoginController : Controller
    {
        /// <summary>
        /// Authenticates the given User model.
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns>The appropriate view after the validation completes.</returns>
        [HttpPost]
        public ActionResult Authenticate(User userModel)
        {
            var _userValidationProvider = new UserValidationProvider();

            if (_userValidationProvider.VerifyUserExists(userModel))
            {
                ModelState.AddModelError("", "The provided Username does not exist.");
                return View("_Login", userModel);
            }

            if (_userValidationProvider.VerifyUserCredentials(userModel))
            {
                ModelState.AddModelError("", "The provided password was incorrect.");
                return View("_Login", userModel);
            }

            FormsAuthentication.SetAuthCookie(userModel.Username, false);
            return RedirectToAction("Index", "Home");
        }
    }
}
