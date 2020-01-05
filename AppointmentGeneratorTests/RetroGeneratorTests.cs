using Appointments;
using FluentAssertions;
using System;
using Xunit;

namespace AppointmentGeneratorTests
{
    public class RetroGeneratorTests
    {
        [Fact]
        public void ShouldFindTwoRetrosInJanuary2020()
        {
            //arrange
            var gen = new RetroGenerator();
            DateTime startDate = new DateTime(2020, 1, 1);
            DateTime endDate = new DateTime(2020, 1, 31);
            //act
            var retros = gen.GetAppointments(startDate, endDate);
            //assert
            retros.Count.Should().Be(2);
        }
    }
}
