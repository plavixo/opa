using Appointments;
using FluentAssertions;
using Moq;
using System;
using Xunit;

namespace AppointmentGeneratorTests
{
    public class AspectResolverTests :IClassFixture<AspectResolverTestsFixture>
    {
        private readonly IAppointment _appointment;
        private readonly Room _location;
        private readonly DateTime _startTime;
        private readonly DateTime _endTime;
        private readonly string _subject;

        public AspectResolverTests(AspectResolverTestsFixture fixture)
        {
            _appointment = fixture.Appointment;
            _startTime = fixture.StartTime;
            _location = fixture.Location;
            _endTime = fixture.EndTime;
            _subject = fixture.Subject;
        }

        [Fact]
        public void ShouldFlattenStartTime() {
            _appointment.StartTime.Should().Be(_startTime);
        }

        [Fact]
        public void ShouldFlattenEndTime()
        {
            _appointment.EndTime.Should().Be(_endTime);
        }

        [Fact]
        public void ShouldFlattenLocation()
        {
            _appointment.Location.Should().Be(_location);
        }

        [Fact]
        public void ShouldFlattenSubject()
        {
            _appointment.Subject.Should().Be(_subject);
        }
    }

    public class AspectResolverTestsFixture
    {
        public IAppointment Appointment { get; }
        public string Subject { get; }
        public DateTime StartTime { get;}
        public DateTime EndTime { get; }
        public Room Location { get; }

        public AspectResolverTestsFixture() {

            StartTime = new DateTime(2020, 1, 2, 13, 00, 00);
            EndTime = new DateTime(2020, 1, 2, 15, 00, 00);
            Location = Room.Alpha;
            Subject = "a subject";
           
            var mockAvailabilityAdaptor = new Mock<IRoomAvailabilityAdaptor>();
            mockAvailabilityAdaptor.SetReturnsDefault(true);
            IRoomAvailabilityAdaptor roomAvailabilityAdaptor = mockAvailabilityAdaptor.Object;

            IAppointmentAspect appointmentWithSubject = new AppointmentWithSubject(Subject,null);
            IAppointmentAspect appointmentWithTime = new AppointmentWithTimes(StartTime, EndTime, appointmentWithSubject);
            IAppointmentAspect appointmentWithLocation = new AppointmentWithLocations(Location, appointmentWithTime);
            
            Appointment = new AppointmentAspectResolver(roomAvailabilityAdaptor).Flatten_PrioritiseTime(appointmentWithLocation);

        }
    }
}
