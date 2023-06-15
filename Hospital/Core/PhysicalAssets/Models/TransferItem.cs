namespace Hospital.Core.PhysicalAssets.Models;

public class TransferItem
{
    public TransferItem()
    {
        TransferId = "";
        Equipment = new Equipment();
        EquipmentId = "";
    }

    public TransferItem(Equipment equipment, int amount, string transferId)

    {
        TransferId = transferId;
        Equipment = equipment;
        Amount = amount;
        EquipmentId = equipment.Id;
    }

    public TransferItem(Equipment equipment, int amount)
    {
        TransferId = "";
        Equipment = equipment;
        Amount = amount;
        EquipmentId = equipment.Id;
    }


    public string TransferId { get; set; }
    public Equipment Equipment { get; set; }
    public string EquipmentId { get; set; }
    public int Amount { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != typeof(TransferItem)) return false;

        var other = (TransferItem)obj;
        return other.Equipment.Equals(Equipment) && other.TransferId.Equals(TransferId);
    }
}