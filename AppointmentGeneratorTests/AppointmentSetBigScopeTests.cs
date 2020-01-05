using Appointments;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            _demo.Location.Should().Be(Room.Bravo);
        }

        [Fact]
        public void MIPShouldBeInRoomBravo()
        {
            _mip.Location.Should().Be(Room.Bravo);
        }

        [Fact]
        public void PlanningShouldBeInRoomBravo()
        {
            _planning.Location.Should().Be(Room.Bravo);
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
            var demoToProcess = new DemoGenerator().GetAppointmentsThatFallWithin(StartDate, EndDate).Single();
            var mipToProcess = new MIPGenerator().GetAppointmentsThatFallWithin(StartDate, EndDate).Single();
            var planningToProcess = new PlanningGenerator().GetAppointmentsThatFallWithin(StartDate, EndDate).Single();
            IList<IAppointmentBuildable> potentialAppointments = new List<IAppointmentBuildable>() {
                demoToProcess, mipToProcess, planningToProcess
            };

            Mock<IRoomAvailabilityAdaptor> mockRoomAvailabilityAdaptor = new Mock<IRoomAvailabilityAdaptor>();
            mockRoomAvailabilityAdaptor.Setup(a => a.RoomIsAvailbleAtTime(It.IsAny<Room>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(true);
            mockRoomAvailabilityAdaptor.Setup(a => a.RoomIsAvailbleAtTime(Room.Alpha, mipToProcess.StartTime, mipToProcess.EndTime))
                .Returns(false);



            IRoomAvailabilityAdaptor roomAvailabilityAdaptor = mockRoomAvailabilityAdaptor.Object;

            IList<Room> desiredRooms = new List<Room> { Room.Alpha, Room.Bravo};
            IEnumerable<IAppointment> appointments = new Prioritiser(roomAvailabilityAdaptor).FlattenSet_FixedTimes_SameRoom(potentialAppointments, desiredRooms);

            Demo = appointments.Where(a => a.Subject.Equals(TestTypes.DemoTitle)).Single();
            MIP = appointments.Where(a => a.Subject.Equals(TestTypes.MIPTitle)).Single();
            Planning = appointments.Where(a => a.Subject.Equals(TestTypes.PlanningTitle)).Single();
        }
    }
}
