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

        public IList<AppointmentWithTimes> GetAppointments(DateTime startDate, DateTime endDate ) {

            IList<AppointmentWithTimes> appointments = new List<AppointmentWithTimes>();

            
            var dates = _potentialDates.Where(date => date > startDate && date < endDate);

            foreach (var date in dates) {
                appointments.Add(
                    new AppointmentWithTimes(date, null)    
                );
            }

            return appointments;
        }

    }
}
