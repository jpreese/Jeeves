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
        public void UsersDoesNotExist()
        {
            // Arrange
            var sut = new Mock<IUserValidationProvider>();

            // test user to pass into the authenticate method
            var testUser = new User
            {
                Username = "testuser",
                Password = "abc123"
            };

            sut.Setup(auth => auth.VerifyUserExists(testUser)).Returns<User>(null);

            // Act
            var authResult = sut.Object.VerifyUserExists(testUser);

            // Assert
            Assert.IsFalse(authResult);
        }
    }
}
