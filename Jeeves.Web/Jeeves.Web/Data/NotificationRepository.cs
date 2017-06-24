using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jeeves.Web.Models;

namespace Jeeves.Web.Data
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ITemperatureRepository _temperatureRepository;

        /// <summary>
        /// Notification Repository constructor.
        /// </summary>
        /// <param name="temperatureRepository">A temperature repository.</param>
        public NotificationRepository(ITemperatureRepository temperatureRepository)
        {
            _temperatureRepository = temperatureRepository;
        }

        /// <summary>
        /// Gets a notification to display to the user.
        /// </summary>
        /// <returns>A notification object representing a notification.</returns>
        public Notification GetNotification()
        {
            var notification = new Notification();
            IEnumerable<Temperature> temps = _temperatureRepository.GetLatestTemperatureReadings();

            if(RecentTemperatureExceedsThreshold(temps))
            {
                notification.NotificationType = NotificationType.Alert;
                notification.NotificationText = "One or more temperatures exceed the threshold.";
            }
            else
            {
                notification.NotificationType = NotificationType.Information;
                notification.NotificationText = "The system is running as expected.";
            }

            return notification;
        }

        /// <summary>
        /// Checks a list of temperatures to see if any exceed the preset threshold.
        /// </summary>
        /// <param name="temperatureList"></param>
        /// <returns>True if a temperature exceeds the threshold, false otherwise.</returns>
        internal bool RecentTemperatureExceedsThreshold(IEnumerable<Temperature> temperatureList)
        {
            // these values are baselines to provide notifications
            const int MAX_COMFORTABLE_TEMP = 80;
            const int MIN_COMFORTABLE_TEMP = 60;

            var listSize = temperatureList.Where(t => t.Reading > MAX_COMFORTABLE_TEMP || t.Reading < MIN_COMFORTABLE_TEMP).Count();
            return listSize > 0;
        }

    }
}