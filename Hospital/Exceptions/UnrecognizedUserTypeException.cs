using System;

namespace Hospital.Exceptions;

public class UnrecognizedUserTypeException : Exception
{
    public UnrecognizedUserTypeException()
    {
    }

    public UnrecognizedUserTypeException(string message) : base(message)
    {
    }
}