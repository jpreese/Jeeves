using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jeeves.Web.Models;

namespace Jeeves.Web.Data
{
    public interface INotificationRepository
    {
        Notification GetNotification();
    }
}