﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareInfromationSystem.Core.Appointment.Notification
{
    interface INotificationRepo
    {
        string GetUnreceived(string userId);
        void MarkRecieved(string userId);
    }
}
