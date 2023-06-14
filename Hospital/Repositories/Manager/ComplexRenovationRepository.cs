using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hospital.Models.Manager;
using Hospital.Serialization;

namespace Hospital.Repositories.Manager;

public class ComplexRenovationRepository
{
    private const string FilePath = "../../../Data/complexRenovations.json";

    private readonly ISerializer<ComplexRenovation> _serializer;
    private List<ComplexRenovation>? _complexRenovations;

    public ComplexRenovationRepository(ISerializer<ComplexRenovation> serializer)
    {
        _serializer = serializer;
    }

    public List<ComplexRenovation> GetAll()
    {
        return _complexRenovations ?? GetAllFromFile();
    }

    private void JoinTransfersWithRooms(List<ComplexRenovation> complexRenovations)
    {
        foreach (var transfer in complexRenovations.SelectMany(renovation => renovation.TransfersFromOldToNewRooms))
        {
            transfer.Origin = RoomRepository.Instance.GetById(transfer.OriginId) ??
                              throw new InvalidOperationException(
                                  "Could not find origin of complex renovation transfer");
            transfer.Destination = RoomRepository.Instance.GetById(transfer.DestinationId) ??
                                   throw new InvalidOperationException(
                                       "Could not find destination of complex renovation transfer");
        }
    }

    private void JoinWithRoomsToDemolish(List<ComplexRenovation> complexRenovations)
    {
        foreach (var renovation in complexRenovations)
            renovation.ToDemolish = RoomRepository.Instance.Get(renovation.ToDemolish);
    }

    private void JoinWithRoomsToBuild(List<ComplexRenovation> complexRenovations)
    {
        foreach (var renovation in complexRenovations)
            renovation.ToBuild = RoomRepository.Instance.Get(renovation.ToBuild);
    }

    private void JoinWithLeftoverEquipmentDestination(List<ComplexRenovation> complexRenovations)
    {
        foreach (var complexRenovation in complexRenovations)
        {
            var leftoverEquipmentDestination =
                complexRenovation.LeftoverEquipmentDestination ?? throw new NullReferenceException();
            complexRenovation.LeftoverEquipmentDestination =
                RoomRepository.Instance.GetById(leftoverEquipmentDestination.Id) ??
                throw new InvalidOperationException("Could not find leftover equipment destination");
        }
    }

    public List<ComplexRenovation> GetAllFromFile()
    {
        try
        {
            _complexRenovations = _serializer.Load(FilePath);
            JoinTransfersWithRooms(_complexRenovations);
            JoinWithRoomsToDemolish(_complexRenovations);
            JoinWithRoomsToBuild(_complexRenovations);
            JoinWithLeftoverEquipmentDestination(_complexRenovations);
            return _complexRenovations;
        }
        catch (Exception ex)
        {
            if (ex is FileNotFoundException)
                return _complexRenovations = new List<ComplexRenovation>();
            throw;
        }
    }

    public void Add(ComplexRenovation complexRenovation)
    {
        _complexRenovations = GetAll();
        _complexRenovations.Add(complexRenovation);
        SaveFile();
    }

    private void SaveFile()
    {
        if (_complexRenovations != null) _serializer.Save(_complexRenovations, FilePath);
    }

    public void Add(List<ComplexRenovation> complexRenovations)
    {
        complexRenovations.ForEach(Add);
    }

    public void Update(ComplexRenovation complexRenovation)
    {
        var complexRenovations = GetAll();
        var indexToUpdate = complexRenovations.FindIndex(e => e.Id == complexRenovation.Id);
        if (indexToUpdate == -1)
            throw new KeyNotFoundException();

        complexRenovations[indexToUpdate] = complexRenovation;
        SaveFile();
    }
}