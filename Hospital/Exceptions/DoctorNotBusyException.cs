using System;

namespace Hospital.Exceptions
{
    public class DoctorNotBusyException : Exception
    {
        public DoctorNotBusyException(string message) : base(message) { }
    }
}
