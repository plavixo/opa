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
    }

    public class AppointmentWithTimes : IAppointmentBuildable
    {
        public IAppointmentBuildable InnerAppointment { get; }
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
        public IEnumerable<Room> Locations { get; }


        public AppointmentWithTimes(DateTime startTime, DateTime endTime, IAppointmentBuildable innerAppointment)

        {
            this.StartTime = startTime;
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

        public AppointmentWithLocations(Room room, IAppointmentBuildable innerAppointment)
        {
            Locations = new List<Room> { room };
            InnerAppointment = innerAppointment;
        }
    }

   

    
}