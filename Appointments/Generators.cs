using System;
using System.Collections.Generic;
using System.Text;

namespace Appointments
{
    public class RetroGenerator : IAppointmentGenerator
    {
        IList<Room>_desirableLocations = new List<Room>() { Room.Alpha, Room.Bravo};
        int _interval = 14;
        DateTime _rootDate = new DateTime(2020, 1, 9, 14, 00, 00);
    
        public IList<IAppointmentBuildable> GetAppointmentsThatFallWithin(DateTime startDate, DateTime endDate)
        {

            IAppointmentGenerator appointmentGenerator = new AppointmentGenerator(_desirableLocations, _interval, _rootDate);
            
            var appointmentsThatFallWithin = appointmentGenerator.GetAppointmentsThatFallWithin(startDate, endDate);
            return appointmentsThatFallWithin;
        }
    }

    public class DemoGenerator : IAppointmentGenerator
    {
        IList<Room> _desirableLocations = new List<Room>() { Room.Alpha, Room.Bravo };
        int _interval = 14;
        DateTime _rootDate = new DateTime(2020, 1, 16, 14, 00, 00);

        public IList<IAppointmentBuildable> GetAppointmentsThatFallWithin(DateTime startDate, DateTime endDate)
        {
            IAppointmentGenerator appointmentGenerator = new AppointmentGenerator(_desirableLocations, _interval, _rootDate);
            return appointmentGenerator.GetAppointmentsThatFallWithin(startDate, endDate);
        }
    }

    public class MIPGenerator : IAppointmentGenerator
    {
        IList<Room> _desirableLocations = new List<Room>() { Room.Alpha, Room.Bravo };
        int _interval = 14;
        DateTime _rootDate = new DateTime(2020, 1, 16, 14, 30, 00);

        public IList<IAppointmentBuildable> GetAppointmentsThatFallWithin(DateTime startDate, DateTime endDate)
        {
            IAppointmentGenerator appointmentGenerator = new AppointmentGenerator(_desirableLocations, _interval, _rootDate);
            return appointmentGenerator.GetAppointmentsThatFallWithin(startDate, endDate);
        }
    }

    public class PlanningGenerator : IAppointmentGenerator
    {
        IList<Room> _desirableLocations = new List<Room>() { Room.Alpha, Room.Bravo };
        int _interval = 14;
        DateTime _rootDate = new DateTime(2020, 1, 16, 15, 00, 00);

        public IList<IAppointmentBuildable> GetAppointmentsThatFallWithin(DateTime startDate, DateTime endDate)
        {
            IAppointmentGenerator appointmentGenerator = new AppointmentGenerator(_desirableLocations, _interval, _rootDate);
            return appointmentGenerator.GetAppointmentsThatFallWithin(startDate, endDate);
        }
    }
}
