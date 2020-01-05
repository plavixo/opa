using System;
using System.Collections.Generic;

namespace Appointments
{
    public interface IAppointmentBuildable
    {
        IAppointmentBuildable InnerAppointment { get; }
        IEnumerable<Room> Locations { get; }
        string Subject { get; }
        TimeBlock TimeBlock { get; }
    }

    public class AppointmentWithTimes : IAppointmentBuildable
    {

        public IAppointmentBuildable InnerAppointment { get; }
        public IEnumerable<Room> Locations { get; }

        public string Subject { get; }

        public TimeBlock TimeBlock { get; }

        public AppointmentWithTimes(DateTime startTime, DateTime endTime, IAppointmentBuildable innerAppointment)

        {
            TimeBlock = new TimeBlock(startTime, endTime);
            InnerAppointment = innerAppointment;
        }

        public AppointmentWithTimes(TimeBlock timeBlock, IAppointmentBuildable innerAppointment)
        {
            TimeBlock = timeBlock;
            InnerAppointment = innerAppointment;
        }
    }


    public class AppointmentWithLocations : IAppointmentBuildable
    {
        public IAppointmentBuildable InnerAppointment { get; }
        public IEnumerable<Room> Locations { get; }
        public string Subject { get; }

        public TimeBlock TimeBlock {get;}

        public AppointmentWithLocations(Room room, IAppointmentBuildable innerAppointment)
        {
            Locations = new List<Room> { room };
            InnerAppointment = innerAppointment;
        }

        public AppointmentWithLocations(IEnumerable<Room> desirableLocations, IAppointmentBuildable innerAppointment)
        {
            Locations = desirableLocations;
            InnerAppointment = innerAppointment;
        }
    }

    public class AppointmentWithSubject : IAppointmentBuildable
    {
        public IAppointmentBuildable InnerAppointment { get; }
        public IEnumerable<Room> Locations { get; }

        public string Subject { get; }

        public TimeBlock TimeBlock { get; }

        public AppointmentWithSubject(string subject, IAppointmentBuildable innerAppointment)
        {
            Subject = subject;
            InnerAppointment = innerAppointment;
        }
    }


}