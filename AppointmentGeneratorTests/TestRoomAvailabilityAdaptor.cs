using Appointments;
using System;

namespace AppointmentGeneratorTests
{
    internal class TestRoomAvailabilityAdaptor : IRoomAvailabilityAdaptor
    {
        public bool RoomIsAvailbleAtTime(Room desiredRoom, DateTime startTime, DateTime endTime)
        {
            return true;
        }
    }
}