using Appointments;
using FluentAssertions;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace AppointmentGeneratorTests
{
	public class GivenPreferredTimeNotAvailableInPreferredRoom
	{
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

		[Fact(Skip = "Placeholding for completeness")]
		public void ShouldFallBackToSecondaryTime() { 
		}
	}	
}
