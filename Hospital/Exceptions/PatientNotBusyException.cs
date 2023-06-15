using System;

namespace Hospital.Exceptions;

public class PatientNotBusyException : Exception
{
    public PatientNotBusyException(string message) : base(message)
    {
    }
}