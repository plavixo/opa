﻿using Appointments;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AppointmentGeneratorTests
{
	public class PrioritiseRoomTests
	{
		[Fact]
		public void ShouldPreferFirstRoomInList()
		{
			//arrange
			Mock<IRoomAvailabilityAdaptor> mockAvailabilityAdaptor = new Mock<IRoomAvailabilityAdaptor>();
			mockAvailabilityAdaptor
				.Setup(a => a.RoomIsAvailbleAtTime(It.IsAny<Room>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
				.Returns(true);
			IRoomAvailabilityAdaptor roomAvailabilityAdaptor = mockAvailabilityAdaptor.Object;

			//Act
			IAppointment flattenedAppointment = SetUpAndRun(roomAvailabilityAdaptor);

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

			//Act
			IAppointment flattenedAppointment = SetUpAndRun(roomAvailabilityAdaptor);

			//assert
			flattenedAppointment.Location.Should().Be(Room.Bravo);
		}

		[Fact]
		public void ShouldFallBackToLastRoom()
		{
			//arrange
			Mock<IRoomAvailabilityAdaptor> mockAvailabilityAdaptor = new Mock<IRoomAvailabilityAdaptor>();
			mockAvailabilityAdaptor
				.Setup(a => a.RoomIsAvailbleAtTime(Room.Alpha, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
				.Returns(false);
			mockAvailabilityAdaptor
				.Setup(a => a.RoomIsAvailbleAtTime(Room.Bravo, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
				.Returns(false);
			mockAvailabilityAdaptor
				.Setup(a => a.RoomIsAvailbleAtTime(Room.Charlie, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
				.Returns(true);

			IRoomAvailabilityAdaptor roomAvailabilityAdaptor = mockAvailabilityAdaptor.Object;
			
			//Act
			IAppointment flattenedAppointment = SetUpAndRun(roomAvailabilityAdaptor);

			//assert
			flattenedAppointment.Location.Should().Be(Room.Charlie);
		}

		private static IAppointment SetUpAndRun(IRoomAvailabilityAdaptor roomAvailabilityAdaptor)
		{
			DateTime start = new DateTime(2020, 1, 1);
			DateTime end = new DateTime(2020, 1, 1);
			IList<Room> desirableLocations = new List<Room>() { Room.Alpha, Room.Bravo, Room.Charlie };


			IRecurringAppointment recurringAppointment = new RecurringAppointment(
				subject: "An appointment",
				desirableLocations,
				1,
				new TimeBlock(start, end)
			);

			IAppointmentAspect testAppointment = new RecurringAppointmentExploder(recurringAppointment)
				.GetAppointmentsThatFallWithin(start, end)
				.Single();

			//act
			var flattenedAppointment = new AppointmentAspectResolver(roomAvailabilityAdaptor).Flatten_PrioritiseTime(testAppointment);
			return flattenedAppointment;
		}
	}	
}
