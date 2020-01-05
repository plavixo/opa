using Appointments;
using FluentAssertions;
using System;
using System.Collections.Generic;
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
            IList<IAppointmentBuildable> retros = gen.GetAppointmentsThatFallWithin(startDate, endDate);
            //assert
            retros.Count.Should().Be(2);
        }
    }
}
