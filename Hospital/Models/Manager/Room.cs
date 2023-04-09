using System;

namespace Hospital.Models.Manager;

public class Room
{
    public enum RoomType
    {
        WAREHOUSE,
        OPERATING_ROOM,
        EXAMINATION_ROOM,
        WAITING_ROOM,
        WARD
    }

    public Room()
    {
        Id = Guid.NewGuid().ToString();
        Name = "";
    }

    public Room(string name, RoomType type)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        Type = type;
    }

    public Room(string id, string name, RoomType type)
    {
        Id = id;
        Name = name;
        Type = type;
    }

    public string Id { get; set; }
    public string Name { get; set; }

    public RoomType Type { get; set; }
}