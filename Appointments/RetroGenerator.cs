using System;
using System.Collections.Generic;
using System.Linq;

namespace Appointments
{
    public class RetroGenerator
    {
        private IList<DateTime> _potentialDates;
        private IList<Room> _desirableLocations;

        public RetroGenerator()
        {
            _potentialDates = GeneratePotentialDates();
            _desirableLocations = new List<Room>() { Room.Alpha, Room.Bravo };
        }

        private IList<DateTime> GeneratePotentialDates()
        {
            
            List<DateTime> potentailDates = new List<DateTime>();
            var rootDate = new DateTime(2020, 1, 9, 14, 00, 00);

            potentailDates.Add(rootDate);
            DateTime loopDate = rootDate;

            for (int i = 0; i < 100; i++) {
                loopDate = loopDate.AddDays(14);
                potentailDates.Add(loopDate);
                
            }
            return potentailDates;
        }

        public IList<IAppointmentBuildable> GetAppointments(DateTime startRange, DateTime endDate ) {

            IList<IAppointmentBuildable> retros = new List<IAppointmentBuildable>();


            DateTime inclusiveEndDate = endDate.AddDays(1);
            IEnumerable<DateTime> times = _potentialDates.Where(date => date > startRange && date <= inclusiveEndDate);

            foreach (var startTime in times) {
                IAppointmentBuildable appointmentWithLocations = new AppointmentWithLocations(_desirableLocations, null);
                retros.Add(
                    new AppointmentWithTimes(startTime, startTime.AddHours(1), appointmentWithLocations)    
                );
            }

            return retros;
        }

    }
}
