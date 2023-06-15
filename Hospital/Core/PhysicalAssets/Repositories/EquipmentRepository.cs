﻿using System.Collections.Generic;
using Hospital.Core.PhysicalAssets.Models;
using Hospital.Serialization;

namespace Hospital.Core.PhysicalAssets.Repositories;

public class EquipmentRepository
{
    private const string FilePath = "../../../Data/equipment.csv";
    private static EquipmentRepository? _instance;

    private List<Equipment>? _equipment;

    private EquipmentRepository()
    {
    }

    public static EquipmentRepository Instance => _instance ??= new EquipmentRepository();

    public List<Equipment> GetAll()
    {
        _equipment ??= CsvSerializer<Equipment>.FromCSV(FilePath);
        return _equipment;
    }

    public List<Equipment> GetNonDynamic()
    {
        return GetAll().FindAll(e => e.Type != EquipmentType.DynamicEquipment);
    }

    public List<Equipment> GetDynamic()
    {
        return GetAll().FindAll(e => e.Type == EquipmentType.DynamicEquipment);
    }

    public Equipment? GetById(string id)
    {
        return GetAll().Find(equipment => equipment.Id == id);
    }

    public void Add(Equipment equipment)
    {
        var allEquipment = GetAll();

        allEquipment.Add(equipment);

        CsvSerializer<Equipment>.ToCSV(allEquipment, FilePath);
    }

    public void Update(Equipment equipment)
    {
        var allEquipment = GetAll();

        var indexToUpdate = allEquipment.FindIndex(e => e.Id == equipment.Id);
        if (indexToUpdate == -1) throw new KeyNotFoundException();

        allEquipment[indexToUpdate] = equipment;

        CsvSerializer<Equipment>.ToCSV(allEquipment, FilePath);
    }

    public void Delete(Equipment equipment)
    {
        var allEquipment = GetAll();

        var indexToDelete = allEquipment.FindIndex(e => e.Id == equipment.Id);
        if (indexToDelete == -1) throw new KeyNotFoundException();

        allEquipment.RemoveAt(indexToDelete);

        CsvSerializer<Equipment>.ToCSV(allEquipment, FilePath);
    }

    public void DeleteAll()
    {
        var equipment = _equipment ?? GetAll();
        equipment.Clear();
        CsvSerializer<Equipment>.ToCSV(equipment, FilePath);
        _equipment = null;
    }
}