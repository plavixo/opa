using System;
using System.Collections.Generic;

namespace Appointments
{
    public class Retro : IRecurringAppointment
    {
        public string Subject { get; } = "Retro";
        public IEnumerable<Room> DesirableLocations { get; } = new List<Room>() { Room.Alpha, Room.Bravo };
        public int Interval { get; } = 14;

        public TimeBlock RootTimeBlock { get; } = 
            new TimeBlock(
                startTime: new DateTime(2020, 1, 9, 14, 00, 00),
                endTime: new DateTime(2020, 1, 9, 15, 00, 00)
            );
    }


    public class Demo : IRecurringAppointment
    {
        public string Subject { get; } = "Demo";
        public IEnumerable<Room> DesirableLocations { get; } = new List<Room>() { Room.Alpha, Room.Bravo };
        public int Interval { get; } = 14;
        public TimeBlock RootTimeBlock { get; } = 
            new TimeBlock(
                startTime: new DateTime(2020, 1, 16, 14, 00, 00),
                endTime: new DateTime(2020, 1, 16, 14, 30, 00)
            );
    }

    public class MIP : IRecurringAppointment
    {
        public string Subject { get; } = "MIP";
        public IEnumerable<Room> DesirableLocations { get; } = new List<Room>() { Room.Alpha, Room.Bravo };
        public int Interval { get; } = 14;
        public TimeBlock RootTimeBlock { get; } = 
            new TimeBlock(
                startTime: new DateTime(2020, 1, 16, 14, 30, 00),
                endTime: new DateTime(2020, 1, 16, 15, 00, 00)
            );
    }

    public class Planning : IRecurringAppointment
    {
        public string Subject { get; } = "Planning";
        public IEnumerable<Room> DesirableLocations { get; } = new List<Room>() { Room.Alpha, Room.Bravo };
        public int Interval { get; } = 14;
        public TimeBlock RootTimeBlock { get; } = 
            new TimeBlock(
                startTime: new DateTime(2020, 1, 16, 15, 00, 00),
                endTime: new DateTime(2020, 1, 16, 15, 30, 00)
            );
    }
}
