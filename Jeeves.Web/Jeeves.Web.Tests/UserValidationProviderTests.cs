using System;
using Jeeves.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Jeeves.Web.Controllers;
using Jeeves.Web.Data;

namespace Jeeves.Web.Tests
{
    [TestClass]
    public class UserValidationProviderTests
    {
        [TestMethod]
        public void UserPasswordDoesNotMatch()
        {
            // Arrange
            var _userRepository = new Mock<IUserRepository>();
            var sut = new UserValidationProvider(_userRepository.Object);

            // mock user to be returned by entity
            var mockUser = new User
            {
                Username = "testuser",
                Password = "abc123"
            };

            // credentials typed in by the user
            var testUser = new User
            {
                Username = "testuser",
                Password = "thisisthewrongpassword"
            };

            _userRepository.Setup(u => u.GetUserByUsername(mockUser.Username)).Returns(mockUser);

            // Act
            var authResult = sut.VerifyUserCredentials(testUser);

            // Assert
            Assert.IsFalse(authResult);
        }
    }
}
