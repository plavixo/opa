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
            TimeBlock timeBlock = GetTimeBlock(appointment);
            
            string subject = GetSubject(appointment);
            IAppointment flattenedAppointment = new Appointment(timeBlock.StartTime, room, subject);

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

        private TimeBlock GetTimeBlock(IAppointmentBuildable appointment)
        {
            TimeBlock timeBlock  = appointment.TimeBlock;

            while (timeBlock == null && appointment.InnerAppointment != null)
            {
                appointment = appointment.InnerAppointment;
                timeBlock = appointment.TimeBlock;
            }

            if (timeBlock == null) throw new Exception($"TimeBlock has not been set");

            return timeBlock;
        }

        private bool RoomAvailableForAll(IList<IAppointmentBuildable> appointments, Room room)
        {
            bool available = true;

            foreach (var appointment in appointments)
            {
                var timeBlock = GetTimeBlock(appointment);
                var startTime = timeBlock.StartTime;
                var endTime = timeBlock.EndTime;

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


        
        private Room GetRoomFor_WithRespectToTimes(IAppointmentBuildable appointment)
        {
            Room room = Room.NotSet;
            
            var loopAppointment = appointment;
            IEnumerable<Room> desiredRooms = GetDesiredRooms(appointment);

            TimeBlock timeBlock = GetTimeBlock(appointment);

            foreach (var desiredRoom in desiredRooms) {             
                if (RoomAvailable(desiredRoom, timeBlock.StartTime, timeBlock.EndTime))
                {
                    room = desiredRoom;
                    break;
                }
            }

            return room;
        }

        private IEnumerable<Room> GetDesiredRooms(IAppointmentBuildable appointment)
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
                string subject = GetSubject(desiredAppointment);
                appointments.Add(
                    new Appointment(
                        desiredAppointment.TimeBlock.StartTime, bestAvailableRoom, subject
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
