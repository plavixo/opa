using Appointments;
using FluentAssertions;
using System;
using Xunit;

namespace AppointmentGeneratorTests
{
    public class FlattenerTests :IClassFixture<FlattenerTestsFixture>
    {
        private readonly IAppointment _appointment;
        private readonly DateTime _startTime;
        private readonly Room _location;

        public FlattenerTests(FlattenerTestsFixture fixture)
        {
            _appointment = fixture.Appointment;
            _startTime = fixture.StartTime;
            _location = fixture.Location;
        }

        [Fact]
        public void ShouldFlattenTimes() {
            _appointment.StartTime.Should().Be(_startTime);
        }

        [Fact]
        public void ShouldFlattenLocation()
        {
            _appointment.Location.Should().Be(_location);
        }
    }

    public class FlattenerTestsFixture
    {
        public IAppointment Appointment { get; }
        public DateTime StartTime { get;}
        public DateTime EndTime { get; }
        public Room Location { get; }

        public FlattenerTestsFixture() {

            StartTime = new DateTime(2020, 1, 2, 13, 00, 00);
            EndTime = new DateTime(2020, 1, 2, 15, 00, 00);
            Location = Room.Alpha;
            IRoomAvailabilityAdaptor roomAvailabilityAdaptor = new TestRoomAvailabilityAdaptor();

            IAppointmentBuildable appointmentWithSubject = new AppointmentWithSubject("an appointment",null);
            IAppointmentBuildable appointmentWithTime = new AppointmentWithTimes(StartTime, EndTime, appointmentWithSubject);
            IAppointmentBuildable appointmentWithLocation = new AppointmentWithLocations(Location, appointmentWithTime);
            Appointment = new Prioritiser(roomAvailabilityAdaptor).Flatten_PrioritiseTime(appointmentWithLocation);

        }
    }
}
