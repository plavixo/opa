using System;

namespace Appointments
{
    public interface IAppointmentBuildable
    {
        IAppointmentBuildable InnerAppointment { get; }
        Room Location { get; }
    }

    public class AppointmentWithTimes : IAppointmentBuildable
    {
        public DateTime StartTime { get; }
        public IAppointmentBuildable InnerAppointment { get; }

        public Room Location { get; } = Room.NotSet;

        public AppointmentWithTimes(DateTime startTime, IAppointmentBuildable innerAppointment)

        {
            this.StartTime = startTime;
            InnerAppointment = innerAppointment;
        }
     }


    public class AppointmentWithLocation : IAppointmentBuildable
    {
        public Room Location { get; }
        public IAppointmentBuildable InnerAppointment { get; }

        public AppointmentWithLocation(Room room, IAppointmentBuildable innerAppointment)
        {
            Location = room;
            InnerAppointment = innerAppointment;
        }
    }

   

    
}