﻿using Appointments;
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
		public void ShouldPreferFirstRoomInList()
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
			IAppointmentBuildable appointmentWithSubject = new AppointmentWithSubject($"another appointment");
			IAppointmentBuildable appointmentWithLocations = new AppointmentWithLocations(locations,appointmentWithSubject);
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

			DateTime start	= new DateTime(2020, 1, 1);
			DateTime end	= new DateTime(2020, 1, 1);
			IEnumerable<Room> desirableLocations = new List<Room>() { Room.Alpha, Room.Bravo };
			IRecurringAppointment recurringAppointment = new RecurringAppointment(
				"an appointment",
				desirableLocations,
				1,
				new TimeBlock(start, end)
			);
			IAppointmentBuildable testAppointment = new AppointmentGenerator(recurringAppointment)
				.GetAppointmentsThatFallWithin(start, end)
				.Single();

			//act
			var flattenedAppointment = new Prioritiser(roomAvailabilityAdaptor).Flatten(testAppointment);

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
				.Returns(false) ;
			mockAvailabilityAdaptor
				.Setup(a => a.RoomIsAvailbleAtTime(Room.Charlie, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
				.Returns(true);

			IRoomAvailabilityAdaptor roomAvailabilityAdaptor = mockAvailabilityAdaptor.Object;

			DateTime start = new DateTime(2020, 1, 1);
			DateTime end = new DateTime(2020, 1, 1);
			IList<Room> desirableLocations = new List<Room>() { Room.Alpha, Room.Bravo, Room.Charlie};


			IRecurringAppointment recurringAppointment = new RecurringAppointment(
				subject: "An appointment",
				desirableLocations,
				1,
				new TimeBlock(start,end)
			);

			IAppointmentBuildable testAppointment = new AppointmentGenerator(recurringAppointment)
				.GetAppointmentsThatFallWithin(start, end)
				.Single();

			//act
			var flattenedAppointment = new Prioritiser(roomAvailabilityAdaptor).Flatten(testAppointment);

			//assert
			flattenedAppointment.Location.Should().Be(Room.Charlie);
		}
	}	
}
