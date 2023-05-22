using System;

namespace Hospital.Models.Manager;

public class Renovation
{
    public Renovation()
    {
        Id = Guid.NewGuid().ToString();
        BeginTime = new DateTime();
        EndTime = new DateTime();
        Completed = false;
        RoomId = "";
    }

    public Renovation(string roomId, DateTime beginTime, DateTime endTime, Room room)
    {
        Id = Guid.NewGuid().ToString();
        RoomId = roomId;
        BeginTime = beginTime;
        EndTime = endTime;
        Completed = false;
        Room = room;
    }

    public Renovation(DateTime beginTime, DateTime endTime, Room room)
    {
        Id = Guid.NewGuid().ToString();
        RoomId = room.Id;
        BeginTime = beginTime;
        EndTime = endTime;
        Completed = false;
        Room = room;
    }

    public string Id { get; set; }

    public string RoomId { get; set; }

    public DateTime BeginTime { get; set; }

    public DateTime EndTime { get; set; }
    public bool Completed { get; set; }

    public Room Room { get; set; }

    public bool OverlapsWith(DateTime date)
    {
        return BeginTime < date && date < EndTime;
    }

    public bool OverlapsWith(DateTime begin, DateTime end)
    {
        return BeginTime < end && begin < EndTime;
    }

    public bool TryComplete()
    {
        if (Completed) return false;

        if (DateTime.Now < EndTime) return false;

        Completed = true;
        return true;
    }
}