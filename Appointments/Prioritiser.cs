using System;
using System.Collections.Generic;
using System.Text;

namespace Appointments
{
    public class Prioritiser
    {
        private readonly IRoomAvailabilityAdaptor _adaptor;

        public Prioritiser(IRoomAvailabilityAdaptor roomAvailabilityAdaptor) {
            _adaptor = roomAvailabilityAdaptor;
        }

        public IAppointment Flatten(IAppointmentBuildable appointment)
        {
            Room room = GetRoomFrom(appointment);
            DateTime startTime = GetStartTime(appointment);

            IAppointment flattenedAppointment = new Appointment(startTime, room);

            return flattenedAppointment;
        }

        private DateTime GetStartTime(IAppointmentBuildable appointment)
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

        private Room GetRoomFrom(IAppointmentBuildable appointment)
        {
            Room room = Room.NotSet;

            Room desiredRoom = appointment.Location;

            while (desiredRoom.Equals(Room.NotSet) && appointment.InnerAppointment!=null) {

                appointment = appointment.InnerAppointment;
                desiredRoom = appointment.Location;            
            }

            if (RoomAvailable(desiredRoom, appointment.StartTime, appointment.EndTime))
            {
                room = desiredRoom;
            }

            return room;
        }

        private bool RoomAvailable(Room desiredRoom, DateTime startTime, DateTime endTime)
        {
            return _adaptor.RoomIsAvailbleAtTime(desiredRoom, startTime, endTime);
        }
    }
}
