using System;
using System.Collections.Generic;
using System.Linq;

namespace Appointments
{
    public class AppointmentGenerator :IAppointmentGenerator
    {
        DateTime _rootDate;

        private IList<DateTime> _potentialDates;
        private IList<Room> _desirableLocations;
        private int _interval;
        private TimeSpan _duration;
        private string _subject;

        public AppointmentGenerator(IList<Room> desirableLocations, int interval, DateTime rootDate, string subject)
        {

            _desirableLocations = desirableLocations;
            _interval = interval;
            _rootDate = rootDate;
            _duration = new TimeSpan(1, 0, 0);
            _subject = subject;


            _potentialDates = GeneratePotentialDates();
        }

        private IList<DateTime> GeneratePotentialDates()
        {
            
            List<DateTime> potentailDates = new List<DateTime>();

            potentailDates.Add(_rootDate);
            DateTime loopDate = _rootDate;

            for (int i = 0; i < 100; i++) {
                loopDate = loopDate.AddDays(_interval);
                potentailDates.Add(loopDate);
                
            }
            return potentailDates;
        }

        public IList<IAppointmentBuildable> GetAppointmentsThatFallWithin(DateTime startOfRange, DateTime EndOfRange ) {

            IList<IAppointmentBuildable> appointments = new List<IAppointmentBuildable>();


            IEnumerable<DateTime> validForStart = _potentialDates.Where(appointmentDate => appointmentDate > startOfRange);
           
            DateTime inclusiveEndOfRange = EndOfRange.AddDays(1);
            IEnumerable<DateTime> times = validForStart.Where(appointmentDate => appointmentDate <= inclusiveEndOfRange);

            foreach (var startTime in times) {
                IAppointmentBuildable appointmentWithSubject = new AppointmentWithSubject(_subject);
                IAppointmentBuildable appointmentWithLocations = new AppointmentWithLocations(_desirableLocations, appointmentWithSubject);
                appointments.Add(
                    new AppointmentWithTimes(startTime, startTime.Add(_duration), appointmentWithLocations)    
                );
            }

            return appointments;
        }

    }

    
}
