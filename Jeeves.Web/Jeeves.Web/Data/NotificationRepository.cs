using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jeeves.Web.Models;

namespace Jeeves.Web.Data
{
    public class NotificationRepository : INotificationRepository
    {
        public Notification GetNotification()
        {
            return CheckForTemperatureNotification();
        }

        private Notification CheckForTemperatureNotification()
        {
            // these values are baselines to provide notifications
            const int MAX_COMFORTABLE_TEMP = 80;
            const int MIN_COMFORTABLE_TEMP = 60;
            var notification = new Notification();

            IEnumerable<Temperature> temps;
            using(var db = new DataContext())
            {
                temps = db.Temperatures.Where(t => t.Reading > MAX_COMFORTABLE_TEMP || t.Reading < MIN_COMFORTABLE_TEMP).ToList();
            }

            if(temps.Count() > 0)
            {
                notification.NotificationType = NotificationType.Alert;
                notification.NotificationText = "One or more temperatures exceed the threshold.";
            }
            else
            {
                notification.NotificationType = NotificationType.Information;
                notification.NotificationText = "System is running as expected.";
            }

            return notification;
        }

    }
}