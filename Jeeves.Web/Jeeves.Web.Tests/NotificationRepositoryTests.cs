using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jeeves.Web.Data;
using Jeeves.Web.Models;
using Moq;

namespace Jeeves.Web.Tests
{
    [TestClass]
    public class NotificationRepositoryTests
    {
        [TestMethod]
        public void HighTemperatureExceedsThreshold()
        {
            // Arrange
            var _temperatureRepository = new Mock<ITemperatureRepository>();
            var sut = new NotificationRepository(_temperatureRepository.Object);
            
            var mockLatestTemps = new List<Temperature>();
            mockLatestTemps.Add(new Temperature { Reading = 60 });
            mockLatestTemps.Add(new Temperature { Reading = 80 });
            mockLatestTemps.Add(new Temperature { Reading = 85 }); // 85 is outside of the 60-80 range

            _temperatureRepository.Setup(t => t.GetLatestTemperatureReadings()).Returns(mockLatestTemps);

            // Act
            var checkTemps = sut.RecentTemperatureExceedsThreshold(mockLatestTemps);

            // Assert
            Assert.IsTrue(checkTemps);
        }

        [TestMethod]
        public void LowTemperatureExceedsThreshold()
        {
            // Arrange
            var _temperatureRepository = new Mock<ITemperatureRepository>();
            var sut = new NotificationRepository(_temperatureRepository.Object);

            var mockLatestTemps = new List<Temperature>();
            mockLatestTemps.Add(new Temperature { Reading = 60 });
            mockLatestTemps.Add(new Temperature { Reading = 80 });
            mockLatestTemps.Add(new Temperature { Reading = 55 }); // 55 is outside of the 60-80 range

            _temperatureRepository.Setup(t => t.GetLatestTemperatureReadings()).Returns(mockLatestTemps);

            // Act
            var checkTemps = sut.RecentTemperatureExceedsThreshold(mockLatestTemps);

            // Assert
            Assert.IsTrue(checkTemps);
        }

        [TestMethod]
        public void SystemNormalDoesNotExceedThreshold()
        {
            // Arrange
            var _temperatureRepository = new Mock<ITemperatureRepository>();
            var sut = new NotificationRepository(_temperatureRepository.Object);

            // for the purpose of this test, all of these temperatures are acceptable
            var mockLatestTemps = new List<Temperature>();
            mockLatestTemps.Add(new Temperature { Reading = 60 });
            mockLatestTemps.Add(new Temperature { Reading = 80 });
            mockLatestTemps.Add(new Temperature { Reading = 70 });

            _temperatureRepository.Setup(t => t.GetLatestTemperatureReadings()).Returns(mockLatestTemps);

            // Act
            var checkTemps = sut.RecentTemperatureExceedsThreshold(mockLatestTemps);

            // Assert
            Assert.IsFalse(checkTemps);
        }
    }
}
