using System;
using System.Collections.Generic;
using System.Linq;

namespace Appointments
{
    public class RetroGenerator
    {
        private IList<DateTime> _potentialDates;

        public RetroGenerator()
        {
            _potentialDates = GeneratePotentialDates();     
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

            
            IEnumerable<DateTime> times = _potentialDates.Where(date => date > startRange && date < endDate);

            foreach (var startTime in times) {
                retros.Add(
                    new AppointmentWithTimes(startTime, startTime.AddHours(1), null)    
                );
            }

            return retros;
        }

    }
}
