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

            string subject = GetSubject(appointment);
            IAppointment flattenedAppointment = new Appointment(startTime, room, subject);

            return flattenedAppointment;
        }

        private string GetSubject(IAppointmentBuildable appointment)
        {
            string subject = appointment.Subject;

            while (subject.IsNullOrEmpty() && appointment.InnerAppointment != null)
            {
                appointment = appointment.InnerAppointment;
                subject = appointment.Subject;
            }

            if (subject.IsNullOrEmpty()) throw new Exception($"Subject has not been set");

            return subject;
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

        private bool RoomAvailableForAll(IList<IAppointmentBuildable> appointments, Room room)
        {
            bool available = true;

            foreach (var appointment in appointments)
            {
                var startTime = GetStartTime(appointment);
                var duration = GetDuration(appointment);
                var endTime = startTime.Add(duration);

                if (RoomAvailable(room, startTime, endTime))
                {
                    continue;
                }
                else
                {
                    available = false;
                }
            }

            return available;

        }

        public IEnumerable<IAppointment> FlattenSet_FixedTimes_SameRoom(IList<IAppointmentBuildable> potentialAppointments, object desiredRooms)
        {
            throw new NotImplementedException();
        }

        private TimeSpan GetDuration(IAppointmentBuildable appointment)
        {
            return new TimeSpan(1, 0, 0);
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

        public IEnumerable<IAppointment> FlattenSet_FixedTimes_SameRoom(IList<IAppointmentBuildable> potentialAppointments, IEnumerable<Room> desiredRooms)
        {
            IList<IAppointment> appointments = new List<IAppointment>();

            Room bestAvailableRoom = Room.NotSet;
            foreach (var room in desiredRooms) {
                if (RoomAvailableForAll(potentialAppointments, room)) {
                    bestAvailableRoom = room;
                }
            }

            foreach (var desiredAppointment in potentialAppointments) {
                string subject = GetSubject(desiredAppointment);
                appointments.Add(
                    new Appointment(
                        desiredAppointment.StartTime, bestAvailableRoom, subject
                    )
                );
            }

            return appointments;
        }

        

        private bool RoomAvailable(Room desiredRoom, DateTime startTime, DateTime endTime)
        {
            return _adaptor.RoomIsAvailbleAtTime(desiredRoom, startTime, endTime);
        }
    }
}
