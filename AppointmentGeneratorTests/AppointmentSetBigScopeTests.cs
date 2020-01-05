using Appointments;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AppointmentGeneratorTests
{
    public class AppointmentSetBigScopeTests :IClassFixture<AppointmentSetBigScopeTestsFixture>
    {
        private IAppointment _demo;
        private IAppointment _mip;
        private IAppointment _planning;

        public AppointmentSetBigScopeTests(AppointmentSetBigScopeTestsFixture fixture) {
            _demo = fixture.Demo;
            _mip = fixture.MIP;
            _planning = fixture.Planning;
        }

        [Fact]
        public void DemoShouldBeInRoomBravo() {
            _demo.Location.Should().Be(Room.Echo);
        }

        [Fact]
        public void MIPShouldBeInRoomBravo()
        {
            _mip.Location.Should().Be(Room.Echo);
        }

        [Fact]
        public void PlanningShouldBeInRoomBravo()
        {
            _planning.Location.Should().Be(Room.Echo);
        }
    }

    public class AppointmentSetBigScopeTestsFixture
    {
        public IAppointment Demo { get; }
        public IAppointment MIP { get; internal set; }
        public IAppointment Planning { get; internal set; }
        public DateTime StartDate { get; } = new DateTime(2020, 1, 13);
        public DateTime EndDate { get; } = new DateTime(2020, 1, 19);

        public AppointmentSetBigScopeTestsFixture()
        {

            //Arrange
            var demoToProcess = new AppointmentGenerator(new Demo()).GetAppointmentsThatFallWithin(StartDate, EndDate).Single();
            var mipToProcess = new AppointmentGenerator(new MIP()).GetAppointmentsThatFallWithin(StartDate, EndDate).Single();
            var planningToProcess = new AppointmentGenerator(new Planning()).GetAppointmentsThatFallWithin(StartDate, EndDate).Single();

            IList<IAppointmentBuildable> setOfAppointments = new List<IAppointmentBuildable>() {
                demoToProcess, mipToProcess, planningToProcess
            };
            
            IList<Room> desiredRoomsForSet = new List<Room> { Room.Delta, Room.Echo };

            IRoomAvailabilityAdaptor roomAvailabilityAdaptor = GetAvailablilityAdaptorThatWillShowRoomBusyAtAppointmentTime(mipToProcess);


            // Act
            IEnumerable<IAppointment> appointments = new Prioritiser(roomAvailabilityAdaptor).FlattenSet_SameRoom_PrioritiseTime(setOfAppointments, desiredRoomsForSet);

            //Unpack for tests
            Demo = appointments.Where(a => a.Subject.Equals(TestTypes.DemoTitle)).Single();
            MIP = appointments.Where(a => a.Subject.Equals(TestTypes.MIPTitle)).Single();
            Planning = appointments.Where(a => a.Subject.Equals(TestTypes.PlanningTitle)).Single();
        }

        private static IRoomAvailabilityAdaptor GetAvailablilityAdaptorThatWillShowRoomBusyAtAppointmentTime(IAppointmentBuildable appointment)
        {
            Mock<IRoomAvailabilityAdaptor> mockRoomAvailabilityAdaptor = new Mock<IRoomAvailabilityAdaptor>();
            mockRoomAvailabilityAdaptor
                .Setup(a => a.RoomIsAvailbleAtTime(It.IsAny<Room>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns((Room room, DateTime start, DateTime end) => thing(room, start, end));

            bool thing(Room room, DateTime start, DateTime end)
            {
                if (room.Equals(Room.Delta) && start.Equals(appointment.TimeBlock.StartTime) && end.Equals(appointment.TimeBlock.EndTime))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            return mockRoomAvailabilityAdaptor.Object;
        }

    }
}
