using System;
using System.Collections.Generic;
using System.Text;

namespace Appointments
{
    public class Prioritiser
    {
        public IAppointment Flatten(IAppointmentBuildable appointment)
        {
            Room room = GetRoomFrom(appointment);

            IAppointment flattenedAppointment = new Appointment(room);

            return flattenedAppointment;
        }

        private Room GetRoomFrom(IAppointmentBuildable appointment)
        {
            Room room = Room.NotSet;

            Room desiredRoom = appointment.Location;

            while (desiredRoom.Equals(Room.NotSet) && !appointment.InnerAppointment.Equals(null)) { 
            
                desiredRoom = appointment.InnerAppointment.Location;            
            }

            if (RoomAvailable(desiredRoom))
            {
                room = desiredRoom;
            }

            return room;
        }

        private bool RoomAvailable(Room desiredRoom)
        {
            return true;
        }
    }
}
