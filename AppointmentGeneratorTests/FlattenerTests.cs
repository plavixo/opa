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
        public IAppointment Appointment { get; internal set; }
        public DateTime StartTime { get; internal set; }
        public Room Location { get; }

        public FlattenerTestsFixture() {

            StartTime = new DateTime(2020, 1, 2);
            Location = Room.Alpha;

            IAppointmentBuildable appointmentWithTime = new AppointmentWithTimes(StartTime, null);
            IAppointmentBuildable appointmentWithLocation = new AppointmentWithLocation(Location, appointmentWithTime);
            Appointment = Prioritiser.Flatten(appointmentWithLocation);

        }
    }
}
