using System.Collections.Generic;

namespace Appointments
{
    public interface IRecurringAppointment
    {
        IEnumerable<Room> DesirableLocations { get; }
        int Interval { get; }
        TimeBlock RootTimeBlock { get; }
        string Subject { get; }
    }

    public class RecurringAppointment :IRecurringAppointment
    {

        public IEnumerable<Room> DesirableLocations { get; }
        public int Interval { get; }
        public TimeBlock RootTimeBlock { get; }
        public string Subject { get; }

        public RecurringAppointment(
            string subject,
            IEnumerable<Room> desirableLocations,
            int intervalDays,
            TimeBlock rootTimeBlock
        )
        {
            DesirableLocations = desirableLocations;
            Interval = intervalDays;
            RootTimeBlock = rootTimeBlock;
            Subject = subject;
        }
    }
}