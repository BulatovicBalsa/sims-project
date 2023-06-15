using System;

namespace Hospital.Exceptions;

public class PatientBusyException : Exception
{
    public PatientBusyException(string message) : base(message)
    {
    }
}