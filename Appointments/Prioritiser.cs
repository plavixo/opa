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
        public IEnumerable<IAppointment> FlattenSet_FixedTimes_SameRoom(IList<IAppointmentBuildable> potentialAppointments, IEnumerable<Room> desiredRooms)
        {
            IEnumerable<TimeBlock> timeBlocks = potentialAppointments.Select(t => FlattenTimeBlock(t));

            Room bestAvailableRoom = GetBestRoomFor(timeBlocks, desiredRooms);
            
            IList<IAppointment> appointments = new List<IAppointment>();
            foreach (var desiredAppointment in potentialAppointments) {

                IAppointmentBuildable wrappedAppointment = new AppointmentWithLocations(bestAvailableRoom, desiredAppointment);
                appointments.Add(Flatten_PrioritiseTime(wrappedAppointment));
            }

            return appointments;
        }

        public IAppointment Flatten_PrioritiseTime(IAppointmentBuildable appointment)
        {
            string subject = FlattenSubject(appointment);
            TimeBlock timeBlock = FlattenTimeBlock(appointment);
            var desiredRooms = FlattenDesiredRooms(appointment);

            Room room = GetBestRoomFor(timeBlock, desiredRooms);
            
            IAppointment flattenedAppointment = new Appointment(
                timeBlock.StartTime, 
                timeBlock.EndTime, 
                room, 
                subject
               );

            return flattenedAppointment;
        }

        private Room GetBestRoomFor(TimeBlock timeBlock, IEnumerable<Room> desiredRooms)
        {
            return GetBestRoomFor(new List<TimeBlock> { timeBlock }, desiredRooms);
        }

        private Room GetBestRoomFor(IEnumerable<TimeBlock> timeBlocks, IEnumerable<Room> desiredRooms)
        {
            Room bestAvailableRoom = Room.NotSet;
          
            foreach (var room in desiredRooms)
            {
                if (RoomAvailableForAll(room,timeBlocks))
                {
                    bestAvailableRoom = room;
                    break;
                }
            }
            return bestAvailableRoom;
        }

        private bool RoomAvailableForAll(Room room, IEnumerable<TimeBlock> timeBlocks)
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
