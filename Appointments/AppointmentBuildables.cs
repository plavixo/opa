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

    public abstract class AppointmentBuildableBase: IAppointmentBuildable { 
        public abstract IAppointmentBuildable InnerAppointment { get; }
        public virtual IEnumerable<Room> Locations { get; }
        public virtual string Subject { get; }
        public virtual TimeBlock TimeBlock { get; }
    }

    public class AppointmentWithTimes :AppointmentBuildableBase
    {

        public override IAppointmentBuildable InnerAppointment { get; }
        public override TimeBlock TimeBlock { get; }

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


    public class AppointmentWithLocations : AppointmentBuildableBase
    {
        public override IAppointmentBuildable InnerAppointment { get; }
        public override IEnumerable<Room> Locations { get; }


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

    public class AppointmentWithSubject : AppointmentBuildableBase
    {
        public override IAppointmentBuildable InnerAppointment { get; }
        public override string Subject { get; }

        public AppointmentWithSubject(string subject, IAppointmentBuildable innerAppointment)
        {
            Subject = subject;
            InnerAppointment = innerAppointment;
        }
    }


}