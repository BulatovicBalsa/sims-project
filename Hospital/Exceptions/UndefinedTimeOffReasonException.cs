using System;

namespace Hospital.Exceptions
{
    public class UndefinedTimeOffReasonException: Exception
    {
        public UndefinedTimeOffReasonException() { }
        public UndefinedTimeOffReasonException(string message) : base(message) { }
    }
}
