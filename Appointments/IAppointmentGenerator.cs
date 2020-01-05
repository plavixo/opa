using System;
using System.Collections.Generic;

namespace Appointments
{
    public interface IAppointmentGenerator
    {
        IList<IAppointmentBuildable> GetAppointmentsThatFallWithin(DateTime startRange, DateTime endDate);
}
}