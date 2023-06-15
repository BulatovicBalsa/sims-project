using System;
using System.Collections.Generic;
using Hospital.Core.PhysicalAssets.Models;
using Hospital.Serialization;

namespace Hospital.Core.PhysicalAssets.Repositories;

public class TransferItemRepository
{
    private const string FilePath = "../../../Data/transferItems.json";
    private static TransferItemRepository? _instance;

    private readonly ISerializer<TransferItem> _serializer;

    private List<TransferItem>? _transferItems;

    public TransferItemRepository(ISerializer<TransferItem> serializer)
    {
        _serializer = serializer;
    }


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
        _transferItems = _serializer.Load(FilePath);
        JoinWithEquipment();
        return _transferItems;
    }

    public void Add(TransferItem item)
    {
        var transferItems = GetAll();
        transferItems.Add(item);
        _serializer.Save(transferItems, FilePath);
    }

    public void Update(TransferItem item)
    {
        var transferItems = GetAll();
        var indexToUpdate = transferItems.FindIndex(e => e.Equals(item));
        if (indexToUpdate == -1)
            throw new KeyNotFoundException();

        transferItems[indexToUpdate] = item;
        _serializer.Save(transferItems, FilePath);
    }

    public void DeleteAll()
    {
        _serializer.Save(new List<TransferItem>(), FilePath);
        _transferItems = null;
    }
}