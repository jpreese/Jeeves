using System;
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
            var userFromEntity = GetUserFromUserContext(userModel);

            if(userFromEntity == null)
            {
                ModelState.AddModelError("", "The provided Username does not exist.");
                return View("_Login", userModel);
            }

            if (userFromEntity.Password != userModel.Password)
            {
                ModelState.AddModelError("", "The provided password was incorrect.");
                return View("_Login", userModel);
            }

            FormsAuthentication.SetAuthCookie(userModel.Username, false);
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Gets a user from the database.
        /// </summary>
        /// <param name="userModel">The user to retrieve.</param>
        /// <returns>User model of the user if one is found, otherwise null.</returns>
        private User GetUserFromUserContext(User userModel)
        {
            using(var db = new UserContext())
            {
                return db.Users.FirstOrDefault(u => u.Username == userModel.Username);
            }
        }

    }
}
