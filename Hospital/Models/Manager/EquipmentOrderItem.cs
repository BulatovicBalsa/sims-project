namespace Hospital.Models.Manager;

public class EquipmentOrderItem
{
    public uint Amount { get; set; }
    public string EquipmentId { get; set; }

    public string OrderId { get; set; }

    public Equipment Equipment { get; set; }

    public EquipmentOrderItem(uint amount, string equipmentId, Equipment equipment)
    {
        Amount = amount;
        EquipmentId = equipmentId;
        OrderId = System.Guid.NewGuid().ToString();
        Equipment = equipment;
    }
}