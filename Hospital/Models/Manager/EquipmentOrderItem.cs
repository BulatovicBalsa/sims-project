namespace Hospital.Models.Manager;

public class EquipmentOrderItem
{
    public int Amount { get; set; }
    public string EquipmentId { get; set; }

    public string OrderId { get; set; }

    public Equipment? Equipment { get; set; }

    public EquipmentOrderItem()
    {
        Amount = 0;
        EquipmentId = "";
        OrderId = "";
    }

    public EquipmentOrderItem(string orderId, int amount, string equipmentId)
    {
        Amount = amount;
        EquipmentId = equipmentId;
        OrderId = orderId;
    }

    public EquipmentOrderItem(string orderId, int amount, Equipment equipment)
    {
        Amount = amount;
        EquipmentId = equipment.Id;
        Equipment = equipment;
        OrderId = orderId;
    }

    public EquipmentOrderItem(int amount, string equipmentId)
    {
        Amount = amount;
        EquipmentId = equipmentId;
    }
}