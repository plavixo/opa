using System;
using System.Collections.Generic;
using System.Text;

namespace Appointments
{
    public class RetroGenerator : IAppointmentGenerator
    {
        IRecurringAppointment retro = new RecurringAppointment(
            subject : "Retro",
            desirableLocations : new List<Room>() { Room.Alpha, Room.Bravo},
            intervalDays : 14,
            rootTimeBlock : new TimeBlock(
                startTime: new DateTime(2020, 1, 9, 14, 00, 00),
                endTime: new DateTime(2020, 1, 9, 15, 00,00)
            )           
        );
    
        public IList<IAppointmentBuildable> GetAppointmentsThatFallWithin(DateTime startDate, DateTime endDate)
        {
            IAppointmentGenerator appointmentGenerator = new AppointmentGenerator(retro);
            
            var appointmentsThatFallWithin = appointmentGenerator.GetAppointmentsThatFallWithin(startDate, endDate);
            
            return appointmentsThatFallWithin;
        }
    }

    public class DemoGenerator : IAppointmentGenerator
    {
        IRecurringAppointment demo = new RecurringAppointment(
            subject: "Demo",
            desirableLocations: new List<Room>() { Room.Alpha, Room.Bravo },
            intervalDays: 14,
            rootTimeBlock: new TimeBlock(
                startTime: new DateTime(2020, 1, 16, 14, 00, 00),
                endTime: new DateTime(2020, 1, 16, 14, 30, 00)
            )
        );

       
        public IList<IAppointmentBuildable> GetAppointmentsThatFallWithin(DateTime startDate, DateTime endDate)
        {
            IAppointmentGenerator appointmentGenerator = new AppointmentGenerator(demo);
            return appointmentGenerator.GetAppointmentsThatFallWithin(startDate, endDate);
        }
    }

    public class MIPGenerator : IAppointmentGenerator
    {
        IRecurringAppointment _mip = new RecurringAppointment(
            subject: "MIP",
            desirableLocations: new List<Room>() { Room.Alpha, Room.Bravo },
            intervalDays: 14,
            rootTimeBlock: new TimeBlock(
                startTime: new DateTime(2020, 1, 16, 14,30, 00),
                endTime: new DateTime(2020, 1, 16, 15, 00, 00)
            )
        );


        public IList<IAppointmentBuildable> GetAppointmentsThatFallWithin(DateTime startDate, DateTime endDate)
        {
            IAppointmentGenerator appointmentGenerator = new AppointmentGenerator(_mip);
            return appointmentGenerator.GetAppointmentsThatFallWithin(startDate, endDate);
        }
    }

    public class PlanningGenerator : IAppointmentGenerator
    {
        IRecurringAppointment _planning = new RecurringAppointment(
            subject: "Planning",
            desirableLocations: new List<Room>() { Room.Alpha, Room.Bravo },
            intervalDays: 14,
            rootTimeBlock: new TimeBlock(
                startTime: new DateTime(2020, 1, 16, 15, 00, 00),
                endTime: new DateTime(2020, 1, 16, 15, 30, 00)
            )
        );


        public IList<IAppointmentBuildable> GetAppointmentsThatFallWithin(DateTime startDate, DateTime endDate)
        {
            IAppointmentGenerator appointmentGenerator = new AppointmentGenerator(_planning);
            return appointmentGenerator.GetAppointmentsThatFallWithin(startDate, endDate);
        }
    }
}
