using System;
using System.Collections.Generic;
using System.Linq;
using Useful.Extensions;

namespace Appointments
{
    public class Prioritiser
    {
        private readonly IRoomAvailabilityAdaptor _adaptor;

        public Prioritiser(IRoomAvailabilityAdaptor roomAvailabilityAdaptor) {
            _adaptor = roomAvailabilityAdaptor;
        }

        public IAppointment Flatten_PrioritiseTime(IAppointmentBuildable appointment)
        {
            TimeBlock timeBlock = FlattenTimeBlock(appointment);
            var desiredRooms = FlattenDesiredRooms(appointment);
       
            Room room = GetBestRoomFor(timeBlock, desiredRooms);
           
            string subject = FlattenSubject(appointment);

            IAppointment flattenedAppointment = new Appointment(timeBlock.StartTime, room, subject);

            return flattenedAppointment;
        }

        private Room GetBestRoomFor(TimeBlock timeBlock, IEnumerable<Room> desiredRooms)
        {
            Room room = Room.NotSet;

            foreach (var desiredRoom in desiredRooms)
            {
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
            IEnumerable<TimeBlock> timeBlocks = potentialAppointments.Select(t => FlattenTimeBlock(t));

            Room bestAvailableRoom = Room.NotSet;
            foreach (var room in desiredRooms) {
                if (RoomAvailableForAll(timeBlocks, room)) {
                    bestAvailableRoom = room;
                    break;
                }
            }

            IList<IAppointment> appointments = new List<IAppointment>();
            foreach (var desiredAppointment in potentialAppointments) {

                IAppointmentBuildable wrappedAppointment = new AppointmentWithLocations(bestAvailableRoom, desiredAppointment);
                appointments.Add(Flatten_PrioritiseTime(wrappedAppointment));
            }

            return appointments;
        }

        private bool RoomAvailableForAll(IEnumerable<TimeBlock> timeBlocks, Room room)
        {
            bool available = true;

            foreach (var timeBlock in timeBlocks)
            {

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
