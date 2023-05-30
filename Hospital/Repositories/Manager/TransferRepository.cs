using System;
using System.Collections.Generic;
using Hospital.Models.Manager;
using Hospital.Serialization;

namespace Hospital.Repositories.Manager;

public class TransferRepository
{
    private const string FilePath = "../../../Data/transfers.csv";

    private static TransferRepository? _instance;

    private List<Transfer>? _transfers;

    private TransferRepository()
    {
    }

    public static TransferRepository Instance => _instance ??= new TransferRepository();

    public List<Transfer> GetAll()
    {
        if (_transfers != null)
            return _transfers;

        _transfers = Serializer<Transfer>.FromCSV(FilePath);

        JoinWithItems();
        JoinWithRooms();

        return _transfers;
    }

    private void WriteItems(Transfer transfer)
    {
        foreach (var item in transfer.Items)
            try
            {
                TransferItemRepository.Instance.Update(item);
            }
            catch (KeyNotFoundException)
            {
                TransferItemRepository.Instance.Add(item);
            }
    }

    private void JoinWithItems()
    {
        var items = TransferItemRepository.Instance.GetAll();
        foreach (var item in items)
        {
            var transfer = _transfers?.Find(e => e.Id == item.TransferId);
            transfer?.AddItem(item);
        }
    }

    private void JoinWithRooms()
    {
        if (_transfers == null) return;
        foreach (var transfer in _transfers)
        {
            transfer.Origin = RoomRepository.Instance.GetById(transfer.OriginId) ??
                              throw new InvalidOperationException();
            transfer.Destination = RoomRepository.Instance.GetById(transfer.DestinationId) ??
                                   throw new InvalidOperationException();
        }
    }


    public void Add(Transfer transfer)
    {
        var transfers = GetAll();
        transfers.Add(transfer);
        Serializer<Transfer>.ToCSV(transfers, FilePath);
        WriteItems(transfer);
    }

    public void Update(Transfer transfer)
    {
        var transfers = GetAll();
        var indexToUpdate = transfers.FindIndex(e => e.Id == transfer.Id);
        if (indexToUpdate == -1)
            throw new KeyNotFoundException();

        transfers[indexToUpdate] = transfer;
        Serializer<Transfer>.ToCSV(transfers, FilePath);
        WriteItems(transfer);
    }

    public void DeleteAll()
    {
        try
        {
            _transfers = GetAll();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        _transfers ??= new List<Transfer>();
        _transfers.Clear();
        Serializer<Transfer>.ToCSV(_transfers, FilePath);
        _transfers = null;
    }

    public void ForceFileReadOnNextCommand()
    {
        _transfers = null;
    }
}