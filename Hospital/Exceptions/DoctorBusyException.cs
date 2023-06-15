using System;

namespace Hospital.Exceptions;

public class DoctorBusyException : Exception
{
    public DoctorBusyException(string message) : base(message)
    {
    }
}