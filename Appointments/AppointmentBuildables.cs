using System;

namespace Appointments
{
    public interface IAppointmentBuildable
    {
        IAppointmentBuildable InnerAppointment { get; }
        Room Location { get; }
        DateTime StartTime { get; }
        DateTime EndTime { get; }
    }

    public class AppointmentWithTimes : IAppointmentBuildable
    {
        public IAppointmentBuildable InnerAppointment { get; }
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }

        public Room Location { get; } = Room.NotSet;


        public AppointmentWithTimes(DateTime startTime, DateTime endTime, IAppointmentBuildable innerAppointment)

        {
            this.StartTime = startTime;
            EndTime = endTime;
            InnerAppointment = innerAppointment;
        }
     }


    public class AppointmentWithLocation : IAppointmentBuildable
    {
        public IAppointmentBuildable InnerAppointment { get; }
        public Room Location { get; }

        public DateTime StartTime { get; } = DateTime.MinValue;

        public DateTime EndTime { get; } = DateTime.MinValue;

        public AppointmentWithLocation(Room room, IAppointmentBuildable innerAppointment)
        {
            Location = room;
            InnerAppointment = innerAppointment;
        }
    }

   

    
}