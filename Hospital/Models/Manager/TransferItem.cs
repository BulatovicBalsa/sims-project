namespace Hospital.Models.Manager;

public class TransferItem
{
    public TransferItem()
    {
        TransferId = "";
        Equipment = new Equipment();
    }

    public TransferItem(string transferId, Equipment equipment, int amount)
    {
        TransferId = transferId;
        Equipment = equipment;
        Amount = amount;
    }
    public TransferItem(Equipment equipment, int amount)
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
        if (obj == null || obj.GetType() != typeof(TransferItem)) return false;

        var other = (TransferItem)obj;
        return other.Equipment.Equals(Equipment) && other.TransferId.Equals(TransferId);
    }
}