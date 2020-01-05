using System;
using System.Collections.Generic;

namespace Appointments
{
    public interface IAppointmentBuildable
    {
        IAppointmentBuildable InnerAppointment { get; }
        IEnumerable<Room> Locations { get; }
        DateTime StartTime { get; }
        DateTime EndTime { get; }
        string Subject { get; }
    }

    public class AppointmentWithTimes : IAppointmentBuildable
    {
        public IAppointmentBuildable InnerAppointment { get; }
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
        public IEnumerable<Room> Locations { get; }

        public string Subject { get; }

        public AppointmentWithTimes(DateTime startTime, DateTime endTime, IAppointmentBuildable innerAppointment)

        {
            StartTime = startTime;
            EndTime = endTime;
            InnerAppointment = innerAppointment;
        }
     }


    public class AppointmentWithLocations : IAppointmentBuildable
    {
        public IAppointmentBuildable InnerAppointment { get; }
        public IEnumerable<Room> Locations { get; }

        public DateTime StartTime { get; } = DateTime.MinValue;

        public DateTime EndTime { get; } = DateTime.MinValue;

        public string Subject { get; }

        public AppointmentWithLocations(Room room, IAppointmentBuildable innerAppointment)
        {
            Locations = new List<Room> { room };
            InnerAppointment = innerAppointment;
        }

        public AppointmentWithLocations(IList<Room> desirableLocations, IAppointmentBuildable innerAppointment)
        {
            Locations = desirableLocations;
            InnerAppointment = innerAppointment;
        }
    }

    public class AppointmentWithSubject : IAppointmentBuildable
    {
        public IAppointmentBuildable InnerAppointment { get; }
        public IEnumerable<Room> Locations { get; }

        public DateTime StartTime { get; } = DateTime.MinValue;

        public DateTime EndTime { get; } = DateTime.MinValue;

        public string Subject { get; }

        public AppointmentWithSubject(string subject)
        {
            Subject = subject;
        }
    }


}