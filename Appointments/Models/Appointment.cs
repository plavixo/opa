using System;

namespace Appointments
{
    public interface IAppointment
    {
        string Subject { get; }
        Room Location { get; }
        DateTime StartTime { get; }
        public DateTime EndTime { get; }
    }

    public class Appointment : IAppointment
    {
        public string Subject { get; }
        public Room Location { get; }
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }

        public Appointment(DateTime startTime, DateTime endTime, Room room, string subject)
        {
            Location = room;
            StartTime = startTime;
            Subject = subject;
            EndTime = endTime;
        }

    }

    public class TimeBlock
    {
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public TimeBlock(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        public TimeBlock(TimeBlock rootTimeBlock)
        {
            StartTime = rootTimeBlock.StartTime;
            EndTime = rootTimeBlock.EndTime;
        }

        internal TimeBlock AddDays(int interval)
        {
            return new TimeBlock(
                startTime: this.StartTime.AddDays(interval),
                endTime: this.EndTime.AddDays(interval)                
            );

        }
    }
}
