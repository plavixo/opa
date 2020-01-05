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
            TimeBlock timeBlock = FlattenTimeBlock(appointment);
            string subject = FlattenSubject(appointment);

            IAppointment flattenedAppointment = new Appointment(timeBlock.StartTime, room, subject);

            return flattenedAppointment;
        }

              
        private Room GetRoomFor_WithRespectToTimes(IAppointmentBuildable appointment)
        {
            Room room = Room.NotSet;
            
            IEnumerable<Room> desiredRooms = FlattenDesiredRooms(appointment);

            TimeBlock timeBlock = FlattenTimeBlock(appointment);

            foreach (var desiredRoom in desiredRooms) {             
                if (RoomAvailable(desiredRoom, timeBlock))
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
                    break;
                }
            }

            foreach (var desiredAppointment in potentialAppointments) {

                IAppointmentBuildable wrappedAppointment = new AppointmentWithLocations(bestAvailableRoom, desiredAppointment);
                appointments.Add(Flatten(wrappedAppointment));
            }

            return appointments;
        }

        private bool RoomAvailableForAll(IList<IAppointmentBuildable> appointments, Room room)
        {
            bool available = true;

            foreach (var appointment in appointments)
            {
                var timeBlock = FlattenTimeBlock(appointment);

                if (RoomAvailable(room, timeBlock))
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



        private bool RoomAvailable(Room desiredRoom, TimeBlock timeBlock)
        {
            return _adaptor.RoomIsAvailbleAtTime(desiredRoom, timeBlock.StartTime, timeBlock.EndTime);
        }



        #region BasicFlattening

        private string FlattenSubject(IAppointmentBuildable appointment)
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

        private TimeBlock FlattenTimeBlock(IAppointmentBuildable appointment)
        {
            TimeBlock timeBlock = appointment.TimeBlock;

            while (timeBlock == null && appointment.InnerAppointment != null)
            {
                appointment = appointment.InnerAppointment;
                timeBlock = appointment.TimeBlock;
            }

            if (timeBlock == null) throw new Exception($"TimeBlock has not been set");

            return timeBlock;
        }

        private IEnumerable<Room> FlattenDesiredRooms(IAppointmentBuildable appointment)
        {
            IEnumerable<Room> desiredRooms = appointment.Locations;

            while (desiredRooms.IsNullOrEmpty() && appointment.InnerAppointment != null)
            {

                appointment = appointment.InnerAppointment;
                desiredRooms = appointment.Locations;
            }

            if (desiredRooms.IsNullOrEmpty()) { throw new Exception($"Desired Rooms not set"); }

            return desiredRooms;
        }

        #endregion BasicFlattening
    }
}
