using Appointments;
using System;

namespace AppointmentGeneratorTests
{
    internal interface IBookingAdaptor
    {
        void Book(IAppointment appointment);
    }
}