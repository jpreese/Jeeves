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
        private readonly IUserRepository _userRepository = new UserRepository();

        public UserValidationProvider(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Validates that the users credentials match
        /// </summary>
        /// <param name="userModel">The usermodel to verify.</param>
        /// <returns>True if the credentials match, false otherwise.</returns>
        public bool VerifyUserCredentials(User userModel)
        {
            var userFromEntity = _userRepository.GetUserByUsername(userModel.Username);
            return userFromEntity.Password == userModel.Password;
        }
    }
}
