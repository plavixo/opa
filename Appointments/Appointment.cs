using System;
using System.Collections.Generic;
using System.Text;

namespace Appointments
{
    public interface IAppointment
    {
        Room Location { get; }
        DateTime StartTime { get; }
    }

    public class Appointment : IAppointment
    {
        public Room Location { get; }

        public DateTime StartTime { get; }

        public Appointment(DateTime startTime, Room room)
        {
            Location = room;
            StartTime = startTime;
        }

    }
}
