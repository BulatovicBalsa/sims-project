using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hospital.Models.Manager;
using Newtonsoft.Json;

namespace Hospital.Repositories.Manager;

public class ComplexRenovationRepository
{
    private const string FilePath = "../../../Data/complexRenovations.json";
    private static ComplexRenovationRepository? _instance;
    private List<ComplexRenovation>? _complexRenovations;

    public static ComplexRenovationRepository Instance =>
        _instance ??= new ComplexRenovationRepository();

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
        foreach (var renovation in complexRenovations) renovation.ToDemolish = GetRooms(renovation.ToDemolish);
    }

    private void JoinWithRoomsToBuild(List<ComplexRenovation> complexRenovations)
    {
        foreach (var renovation in complexRenovations) renovation.ToBuild = GetRooms(renovation.ToBuild);
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

    private List<Room> GetRooms(List<Room> rooms)
    {
        return rooms.Select(room =>
            RoomRepository.Instance.GetById(room.Id) ?? throw new InvalidOperationException()).ToList();
    }

    public List<ComplexRenovation> GetAllFromFile()
    {
        try
        {
            _complexRenovations = JsonConvert.DeserializeObject<List<ComplexRenovation>>(File.ReadAllText(FilePath)) ??
                                  throw new InvalidOperationException();
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
        File.WriteAllText(FilePath, JsonConvert.SerializeObject(_complexRenovations));
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