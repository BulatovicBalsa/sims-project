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

    public string EquipmentId { get; set; }
    public string RoomId { get; set; }

    public int Amount { get; set; }

    public Equipment? Equipment { get; set; }
}