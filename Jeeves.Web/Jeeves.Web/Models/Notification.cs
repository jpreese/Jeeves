using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jeeves.Web.Models
{
    public class Notification
    {
        public string NotificationText { get; set; }
        public NotificationType NotificationType { get; set; }

    }

    /// <summary>
    /// Enumeration of all of the notification types
    /// </summary>
    public enum NotificationType
    {
        Information,
        Warning,
        Alert
    }
}