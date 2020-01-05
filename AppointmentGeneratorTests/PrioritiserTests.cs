using Appointments;
using FluentAssertions;
using System;
using Xunit;

namespace AppointmentGeneratorTests
{
	public class PrioritiserTests
	{
		[Fact]
		public void ShouldPrioritiseOnRooms() {
			//arrange
			DateTime startingTime = DateTime.Now;
			IAppointmentBuildable appointmentWithTime = new AppointmentWithTimes(startingTime, null);
			IAppointmentBuildable startingAppointment = new AppointmentWithLocation(Room.Alpha, appointmentWithTime);

			Prioritiser prioritiser = new Prioritiser();
			
			//act
			IAppointment appointment = prioritiser.Flatten(startingAppointment);
			
			//assert
			appointment.Location.Should().Be(Room.Alpha);
		}

		[Fact]
		public void ShouldGetNestedRoom() {
						//arrange
			DateTime startingTime = DateTime.Now;
			IAppointmentBuildable appointmentWithLocation = new AppointmentWithLocation(Room.Alpha, null);
			IAppointmentBuildable appointmentWithTime = new AppointmentWithTimes(startingTime, appointmentWithLocation);

			Prioritiser prioritiser = new Prioritiser();

			//act
			IAppointment appointment = prioritiser.Flatten(appointmentWithTime);

			//assert
			appointment.Location.Should().Be(Room.Alpha);

		}
	}

	
}
