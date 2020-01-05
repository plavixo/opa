using System;

namespace Appointments
{
    public interface IRoomAvailabilityAdaptor
    {
        bool RoomIsAvailbleAtTime(Room desiredRoom, DateTime startTime, DateTime endTime);
    }
}