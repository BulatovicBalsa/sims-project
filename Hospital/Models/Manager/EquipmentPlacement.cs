namespace Hospital.Models.Manager;

public class EquipmentPlacement
{
    public EquipmentPlacement()
    {
        Amount = 0;
        EquipmentId = "";
        RoomId = "";
    }

    public EquipmentPlacement(string equipmentId, string roomId, int amount)
    {
        EquipmentId = equipmentId;
        RoomId = roomId;
        Amount = amount;
    }

    public EquipmentPlacement(Equipment equipment, string roomId, int amount)
    {
        EquipmentId = equipment.Id;
        RoomId = roomId;
        Amount = amount;
        Equipment = equipment;
    }

    public string EquipmentId { get; set; }
    public string RoomId { get; set; }

    public int Amount { get; set; }

    public Equipment? Equipment { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != typeof(EquipmentPlacement)) return false;

        var otherPlacement = (EquipmentPlacement)obj;
        return EquipmentId == otherPlacement.EquipmentId && RoomId == otherPlacement.RoomId;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}