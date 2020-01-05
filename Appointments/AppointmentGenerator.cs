using System;
using System.Collections.Generic;
using System.Linq;
using Useful.Extensions;

namespace Appointments
{
    public class AppointmentGenerator :IAppointmentGenerator
    {
        DateTime _rootDate;

        private IList<DateTime> _potentialDates;

        private IEnumerable<TimeBlock> _potentialTimeBlocks;

        private IEnumerable<Room> _desirableLocations;
        private int _interval;
        private TimeBlock _rootTimeBlock;
        private string _subject;

        public AppointmentGenerator(IList<Room> desirableLocations, int interval, DateTime rootDate, string subject)
        {

            _desirableLocations = desirableLocations;
            _interval = interval;
            _rootDate = rootDate;
            _subject = subject;


            _potentialDates = GeneratePotentialDates();
        }

        public AppointmentGenerator(IRecurringAppointment recurringAppointment)
        {
            _desirableLocations = recurringAppointment.DesirableLocations;
            _interval = recurringAppointment.Interval;
            _rootTimeBlock = recurringAppointment.RootTimeBlock;
            _subject = recurringAppointment.Subject;


            _potentialTimeBlocks = GeneratePotentialTimeBlocks();

        }

        private IEnumerable<TimeBlock> GeneratePotentialTimeBlocks()
        {
            List<TimeBlock> potentialTimeBlocks = new List<TimeBlock>();

            potentialTimeBlocks.Add(_rootTimeBlock);
            TimeBlock loopBlock = new TimeBlock(_rootTimeBlock);

            for (int i = 0; i < 100; i++)
            {
                loopBlock = loopBlock.AddDays(_interval);
                potentialTimeBlocks.Add(loopBlock);

            }
            return potentialTimeBlocks;
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

            if (_potentialTimeBlocks.IsNullOrEmpty())
            {

                IEnumerable<DateTime> validForStart = _potentialDates.Where(appointmentDate => appointmentDate > startOfRange);

                DateTime inclusiveEndOfRange = EndOfRange.AddDays(1);
                IEnumerable<DateTime> times = validForStart.Where(appointmentDate => appointmentDate <= inclusiveEndOfRange);

                foreach (var startTime in times)
                {
                    IAppointmentBuildable appointmentWithSubject = new AppointmentWithSubject(_subject);
                    IAppointmentBuildable appointmentWithLocations = new AppointmentWithLocations(_desirableLocations, appointmentWithSubject);

                    var timeBlock = new TimeBlock(startTime, startTime.AddHours(1)); //Todo here
                    appointments.Add(
                        new AppointmentWithTimes(timeBlock, appointmentWithLocations)
                    );
                }

                return appointments;
            }
            else {
                IEnumerable<TimeBlock> validForStart = _potentialTimeBlocks.Where(appointmentBlock => appointmentBlock.StartTime > startOfRange);

                DateTime inclusiveEndOfRange = EndOfRange.AddDays(1);
                IEnumerable<TimeBlock> timeBlocks = validForStart.Where(appointmentBlock => appointmentBlock.EndTime <= inclusiveEndOfRange);

                foreach (var block in timeBlocks)
                {
                    IAppointmentBuildable appointmentWithSubject = new AppointmentWithSubject(_subject);
                    IAppointmentBuildable appointmentWithLocations = new AppointmentWithLocations(_desirableLocations, appointmentWithSubject);

                    var timeBlock = new TimeBlock(block.StartTime, block.EndTime);
                    appointments.Add(
                        new AppointmentWithTimes(timeBlock, appointmentWithLocations)
                    );
                }

                return appointments;


            }

        }

    }

    
}
