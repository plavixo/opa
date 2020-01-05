using Appointments;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AppointmentGeneratorTests
{
	public class RoomPriorityTests
	{
		[Fact]
		public void ShouldPreferRoomAlpha()
		{
			//arrange
			Mock<IRoomAvailabilityAdaptor> mockAvailabilityAdaptor = new Mock<IRoomAvailabilityAdaptor>();
			mockAvailabilityAdaptor
				.Setup(a => a.RoomIsAvailbleAtTime(It.IsAny<Room>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
				.Returns(true);
			

			IRoomAvailabilityAdaptor roomAvailabilityAdaptor = mockAvailabilityAdaptor.Object;

			DateTime start = new DateTime(2020, 1, 1);
			DateTime end = new DateTime(2020, 1, 9);

			IList<Room> locations = new List<Room>() { Room.Alpha, Room.Bravo };
			IAppointmentBuildable appointmentWithLocations = new AppointmentWithLocations(locations,null);
			IAppointmentBuildable testAppointment = new AppointmentWithTimes(start, end, appointmentWithLocations);

			//act
			var flattenedAppointment = new Prioritiser(roomAvailabilityAdaptor).Flatten(testAppointment);

			//assert
			flattenedAppointment.Location.Should().Be(Room.Alpha);
		}

		[Fact]
		public void ShouldFallBackToSecondaryRoom() {
			//arrange
			Mock<IRoomAvailabilityAdaptor> mockAvailabilityAdaptor = new Mock<IRoomAvailabilityAdaptor>();
			mockAvailabilityAdaptor
				.Setup(a => a.RoomIsAvailbleAtTime(Room.Alpha, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
				.Returns(false);
			mockAvailabilityAdaptor
				.Setup(a => a.RoomIsAvailbleAtTime(Room.Bravo, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
				.Returns(true);

			IRoomAvailabilityAdaptor roomAvailabilityAdaptor = mockAvailabilityAdaptor.Object;

			DateTime start = new DateTime(2020,1,1);
			DateTime end = new DateTime(2020, 1, 9);
			IAppointmentBuildable testAppointment = new RetroGenerator().GetAppointments(start, end).Single();

			//act
			var flattenedAppointment = new Prioritiser(roomAvailabilityAdaptor).Flatten(testAppointment);

			//assert
			flattenedAppointment.Location.Should().Be(Room.Bravo);
		}
	}	
}
