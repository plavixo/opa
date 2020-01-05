using System;
using System.Collections.Generic;
using System.Text;

namespace Appointments
{
    public interface IAppointment
    {
        Room Location { get; }
        DateTime StartTime { get; }
        string Subject { get; }
    }

    public class Appointment : IAppointment
    {
        public Room Location { get; }

        public DateTime StartTime { get; }
        public string Subject { get; }

        public Appointment(DateTime startTime, Room room, string subject)
        {
            Location = room;
            StartTime = startTime;
            Subject = subject;
        }

    }
}
