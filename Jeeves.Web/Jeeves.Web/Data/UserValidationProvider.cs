using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jeeves.Web.Models;
using Jeeves.Web.Data;

namespace Jeeves.Web.Data
{
    public class UserValidationProvider : IUserValidationProvider
    {
        /// <summary>
        /// Checks to see if the user exists in the database.
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public bool VerifyUserExists(User userModel)
        {
            using (var db = new DataContext())
            {
                return db.Users.First(u => u.Username == userModel.Username) != null ? true : false;
            }
        }

        /// <summary>
        /// Validates that the users credentials match
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool VerifyUserCredentials(User userModel)
        {
            User userFromEntity;
            using (var db = new DataContext())
            {
                userFromEntity = db.Users.First(u => u.Username == userModel.Username);
            }

            return userModel.Password == userFromEntity.Password ? true : false;
        }
    }
}
