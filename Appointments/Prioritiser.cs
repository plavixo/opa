using System;
using System.Collections.Generic;
using Useful.Extensions;

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
            Room room = GetRoomFor_WithRespectToTimes(appointment);
            DateTime startTime = GetStartTime(appointment);

            IAppointment flattenedAppointment = new Appointment(startTime, room);

            return flattenedAppointment;
        }

        public object Flatten(object testAppointment)
        {
            throw new NotImplementedException();
        }

        private DateTime GetStartTime(IAppointmentBuildable appointment)
        {
            DateTime startTime  = appointment.StartTime;

            while (startTime.Equals(DateTime.MinValue) && appointment.InnerAppointment != null)
            {
                appointment = appointment.InnerAppointment;
                startTime = appointment.StartTime;
            }

            if (startTime == DateTime.MinValue) throw new Exception($"Starttime has not been set");

            return startTime;
        }

        private Room GetRoomFor_WithRespectToTimes(IAppointmentBuildable appointment)
        {
            Room room = Room.NotSet;

            IEnumerable<Room> desiredRooms = appointment.Locations;

            while (desiredRooms.IsNullOrEmpty() && appointment.InnerAppointment!=null) {

                appointment = appointment.InnerAppointment;
                desiredRooms = appointment.Locations;            
            }

            foreach (var desiredRoom in desiredRooms) {             
                if (RoomAvailable(desiredRoom, appointment.StartTime, appointment.EndTime))
                {
                    room = desiredRoom;
                    break;
                }
            }

            return room;
        }

        private bool RoomAvailable(Room desiredRoom, DateTime startTime, DateTime endTime)
        {
            return _adaptor.RoomIsAvailbleAtTime(desiredRoom, startTime, endTime);
        }
    }
}
