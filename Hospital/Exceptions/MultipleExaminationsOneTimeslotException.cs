using System;

namespace Hospital.Exceptions;

public class MultipleExaminationsOneTimeslotException : Exception
{
    public MultipleExaminationsOneTimeslotException() : base()
    {
        
    }

    public MultipleExaminationsOneTimeslotException(string message) : base(message)
    {

    }
}