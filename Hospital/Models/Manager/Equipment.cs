using System;

namespace Hospital.Models.Manager;

public class Equipment
{
    public Equipment()
    {
        Id = Guid.NewGuid().ToString();
        Name = "";
    }

    public Equipment(string id, string name, EquipmentType type)
    {
        Id = id;
        Name = name;
        Type = type;
    }


    public Equipment(string name, EquipmentType type)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        Type = type;
    }

    public string Id { get; set; }

    public string Name { get; set; }

    public EquipmentType Type { get; set; }

    public override string ToString()
    {
        return Name;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != typeof(Equipment))
        {
            return false;
        }

        return ((Equipment)obj).Id == Id;
    }
}