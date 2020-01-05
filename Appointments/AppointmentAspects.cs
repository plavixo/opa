using System;
using System.Collections.Generic;

namespace Appointments
{
    public interface IAppointmentAspect
    {
        IAppointmentAspect InnerAppointment { get; }
        IEnumerable<Room> Locations { get; }
        string Subject { get; }
        TimeBlock TimeBlock { get; }
    }

    public abstract class AppointmentAspectBase: IAppointmentAspect { 
        public abstract IAppointmentAspect InnerAppointment { get; }
        public virtual IEnumerable<Room> Locations { get; }
        public virtual string Subject { get; }
        public virtual TimeBlock TimeBlock { get; }
    }

    public class AppointmentWithTimes :AppointmentAspectBase
    {

        public override IAppointmentAspect InnerAppointment { get; }
        public override TimeBlock TimeBlock { get; }

        public AppointmentWithTimes(DateTime startTime, DateTime endTime, IAppointmentAspect innerAppointment)

        {
            TimeBlock = new TimeBlock(startTime, endTime);
            InnerAppointment = innerAppointment;
        }

        public AppointmentWithTimes(TimeBlock timeBlock, IAppointmentAspect innerAppointment)
        {
            TimeBlock = timeBlock;
            InnerAppointment = innerAppointment;
        }
    }


    public class AppointmentWithLocations : AppointmentAspectBase
    {
        public override IAppointmentAspect InnerAppointment { get; }
        public override IEnumerable<Room> Locations { get; }


        public AppointmentWithLocations(Room room, IAppointmentAspect innerAppointment)
        {
            Locations = new List<Room> { room };
            InnerAppointment = innerAppointment;
        }

        public AppointmentWithLocations(IEnumerable<Room> desirableLocations, IAppointmentAspect innerAppointment)
        {
            Locations = desirableLocations;
            InnerAppointment = innerAppointment;
        }
    }

    public class AppointmentWithSubject : AppointmentAspectBase
    {
        public override IAppointmentAspect InnerAppointment { get; }
        public override string Subject { get; }

        public AppointmentWithSubject(string subject, IAppointmentAspect innerAppointment)
        {
            Subject = subject;
            InnerAppointment = innerAppointment;
        }
    }


}