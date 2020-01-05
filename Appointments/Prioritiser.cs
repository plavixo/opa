using System;
using System.Collections.Generic;
using System.Text;

namespace Appointments
{
    public class Prioritiser
    {
        public static IAppointment Flatten(IAppointmentBuildable appointment)
        {
            Room room = GetRoomFrom(appointment);
            DateTime startTime = GetStartTime(appointment);

            IAppointment flattenedAppointment = new Appointment(startTime, room);

            return flattenedAppointment;
        }

        private static DateTime GetStartTime(IAppointmentBuildable appointment)
        {
            DateTime startTime = default;

            startTime = appointment.StartTime;

            while (startTime.Equals(DateTime.MinValue) && appointment.InnerAppointment != null)
            {
                appointment = appointment.InnerAppointment;
                startTime = appointment.StartTime;
            }

            return startTime;
        }

        private static Room GetRoomFrom(IAppointmentBuildable appointment)
        {
            Room room = Room.NotSet;

            Room desiredRoom = appointment.Location;

            while (desiredRoom.Equals(Room.NotSet) && appointment.InnerAppointment!=null) {

                appointment = appointment.InnerAppointment;
                desiredRoom = appointment.Location;            
            }

            if (RoomAvailable(desiredRoom))
            {
                room = desiredRoom;
            }

            return room;
        }

        private static bool RoomAvailable(Room desiredRoom)
        {
            return true;
        }
    }
}
