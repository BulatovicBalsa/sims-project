using System;
using System.Collections.Generic;
using Hospital.Models.Manager;
using Hospital.Serialization;

namespace Hospital.Repositories.Manager;

public class TransferItemRepository
{
    private const string FilePath = "../../../Data/transferItems.csv";
    private static TransferItemRepository? _instance;

    private List<TransferItem>? _transferItems;

    private TransferItemRepository()
    {
    }

    public static TransferItemRepository Instance => _instance ??= new TransferItemRepository();

    private void JoinWithEquipment()
    {
        if (_transferItems == null) return;
        foreach (var item in _transferItems)
            item.Equipment = EquipmentRepository.Instance.GetById(item.EquipmentId) ??
                             throw new InvalidOperationException("Inventory reference in transfer item not found");
    }

    public List<TransferItem> GetAll()
    {
        if (_transferItems != null)
            return _transferItems;
        _transferItems = Serializer<TransferItem>.FromCSV(FilePath);
        JoinWithEquipment();
        return _transferItems;
    }

    public void Add(TransferItem item)
    {
        var transferItems = GetAll();
        transferItems.Add(item);
        Serializer<TransferItem>.ToCSV(transferItems, FilePath);
    }

    public void Update(TransferItem item)
    {
        var transferItems = GetAll();
        var indexToUpdate = transferItems.FindIndex(e => e.Equals(item));
        if (indexToUpdate == -1)
            throw new KeyNotFoundException();

        transferItems[indexToUpdate] = item;
        Serializer<TransferItem>.ToCSV(transferItems, FilePath);
    }

    public void DeleteAll()
    {
        Serializer<TransferItem>.ToCSV(new List<TransferItem>(), FilePath);
        _transferItems = null;
    }
}