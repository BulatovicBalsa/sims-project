using System;

namespace Hospital.Exceptions;

public class MultipleExaminationsOneTimeslotException : Exception
{
    public MultipleExaminationsOneTimeslotException()
    {
    }

    public MultipleExaminationsOneTimeslotException(string message) : base(message)
    {
    }
}