using Newtonsoft.Json;

namespace Hospital.PhysicalAssets.Models;

public class InventoryItem
{
    public InventoryItem()
    {
        Amount = 0;
        EquipmentId = "";
        RoomId = "";
        Reserved = 0;
    }

    public InventoryItem(string equipmentId, string roomId, int amount)
    {
        EquipmentId = equipmentId;
        RoomId = roomId;
        Amount = amount;
        Reserved = 0;
    }

    public InventoryItem(Equipment equipment, string roomId, int amount)
    {
        EquipmentId = equipment.Id;
        RoomId = roomId;
        Amount = amount;
        Equipment = equipment;
        Reserved = 0;
    }

    [JsonProperty("EquipmentId")] public string EquipmentId { get; set; }

    [JsonProperty("RoomId")] public string RoomId { get; set; }

    [JsonProperty("Amount")] public int Amount { get; set; }

    [JsonProperty("Equipment")] public Equipment? Equipment { get; set; }

    [JsonProperty("Reserved")] public int Reserved { get; set; }

    public int Available => Amount - Reserved;

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != typeof(InventoryItem)) return false;

        var otherPlacement = (InventoryItem)obj;
        return EquipmentId == otherPlacement.EquipmentId && RoomId == otherPlacement.RoomId;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}