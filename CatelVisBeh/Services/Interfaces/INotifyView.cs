using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatelVisBeh.Services.Interfaces
{
    public interface INotifyView
    {
        Roles.Roles Role { get; set; }

        event EventHandler Notification;

        void OnNotification();
    }
}
