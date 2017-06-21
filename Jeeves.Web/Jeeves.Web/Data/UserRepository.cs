using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jeeves.Web.Models;

namespace Jeeves.Web.Data
{
    public class UserRepository : IUserRepository
    {
        /// <summary>
        /// Returns the first user in the database given a Username.
        /// </summary>
        /// <param name="userName">The username to look up.</param>
        /// <returns>The user model.</returns>
        public User GetUserByUsername(string userName)
        {
            using(var db = new DataContext())
            {
                return db.Users.Where(u => u.Username == userName).FirstOrDefault();
            }
        }
    }
}
