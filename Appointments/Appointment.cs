using System;
using System.Collections.Generic;
using System.Text;

namespace Appointments
{
    public interface IAppointment
    {
        public Room Location { get; }
    }

    public class Appointment : IAppointment
    {
        public Room Location { get; }

        public Appointment(Room room)
        {
            Location = room;
        }

    }
}
