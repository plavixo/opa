using System;
using System.Collections.Generic;
using System.Linq;

namespace Appointments
{
    public interface IRecurringAppointmentExploder
    {
        IList<IAppointmentAspect> GetAppointmentsThatFallWithin(DateTime startRange, DateTime endDate);
    }

    public class RecurringAppointmentExploder :IRecurringAppointmentExploder
    {

        private IEnumerable<TimeBlock> _potentialTimeBlocks;
        private IEnumerable<Room> _desirableLocations;
        private int _interval;
        private TimeBlock _rootTimeBlock;
        private string _subject;


        public RecurringAppointmentExploder(IRecurringAppointment recurringAppointment)
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

        public IList<IAppointmentAspect> GetAppointmentsThatFallWithin(DateTime startOfRange, DateTime EndOfRange ) {

            IList<IAppointmentAspect> appointments = new List<IAppointmentAspect>();

            
            IEnumerable<TimeBlock> validForStart = _potentialTimeBlocks.Where(appointmentBlock => appointmentBlock.StartTime > startOfRange);

            DateTime inclusiveEndOfRange = EndOfRange.AddDays(1);
            IEnumerable<TimeBlock> timeBlocks = validForStart.Where(appointmentBlock => appointmentBlock.EndTime <= inclusiveEndOfRange);

            foreach (var block in timeBlocks)
            {
                IAppointmentAspect appointmentWithSubject = new AppointmentWithSubject(_subject, null);
                IAppointmentAspect appointmentWithLocations = new AppointmentWithLocations(_desirableLocations, appointmentWithSubject);

                var timeBlock = new TimeBlock(block.StartTime, block.EndTime);
                appointments.Add(
                    new AppointmentWithTimes(timeBlock, appointmentWithLocations)
                );
            }

            return appointments;
        }

    }

    
}
