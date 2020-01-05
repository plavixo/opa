using System;
using System.Collections.Generic;
using System.Text;

namespace Appointments
{
    public interface IAppointment
    {
        Room Location { get; }
        DateTime StartTime { get; }
        string Subject { get; }
    }

    public class Appointment : IAppointment
    {
        public Room Location { get; }

        public DateTime StartTime { get; }
        public string Subject { get; }
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
            this.StartTime = rootTimeBlock.StartTime;
            this.EndTime = rootTimeBlock.EndTime;
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
