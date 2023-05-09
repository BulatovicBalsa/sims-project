namespace Hospital.Models.Manager;

public class EquipmentTransferItem
{
    public EquipmentTransferItem()
    {
        TransferId = "";
        Equipment = new Equipment();
    }

    public EquipmentTransferItem(string transferId, Equipment equipment, int amount)
    {
        TransferId = transferId;
        Equipment = equipment;
        Amount = amount;
    }
    public EquipmentTransferItem(Equipment equipment, int amount)
    {
        TransferId = "";
        Equipment = equipment;
        Amount = amount;
    }
    

    public string TransferId { get; set; }
    public Equipment Equipment { get; set; }
    public int Amount { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != typeof(EquipmentTransferItem)) return false;

        var other = (EquipmentTransferItem)obj;
        return other.Equipment.Equals(Equipment) && other.TransferId.Equals(TransferId);
    }
}