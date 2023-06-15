using System;

namespace Hospital.Exceptions;

public class RoomBusyException : Exception
{
    public RoomBusyException(string message) : base(message)
    {
    }
}