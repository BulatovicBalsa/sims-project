using System.Collections.Generic;
using Hospital.Models.Manager;
using Hospital.Serialization;

namespace Hospital.Repositories.Manager;

public class EquipmentRepository
{
    private const string FilePath = "../../../Data/equipment.csv";

    public List<Equipment> GetAll()
    {
        return Serializer<Equipment>.FromCSV(FilePath);
    }

    public List<Equipment> GetNonDynamic()
    {
        return GetAll().FindAll(e => e.Type != Equipment.EquipmentType.DynamicEquipment);
    }

    public List<Equipment> GetDynamic()
    {
        return GetAll().FindAll(e => e.Type == Equipment.EquipmentType.DynamicEquipment);
    }

    public Equipment? GetById(string id)
    {
        return GetAll().Find(equipment => equipment.Id == id);
    }

    public void Add(Equipment equipment)
    {
        var allEquipment = GetAll();

        allEquipment.Add(equipment);

        Serializer<Equipment>.ToCSV(allEquipment, FilePath);
    }

    public void Update(Equipment equipment)
    {
        var allEquipment = GetAll();

        var indexToUpdate = allEquipment.FindIndex(e => e.Id == equipment.Id);
        if (indexToUpdate == -1) throw new KeyNotFoundException();

        allEquipment[indexToUpdate] = equipment;

        Serializer<Equipment>.ToCSV(allEquipment, FilePath);
    }

    public void Delete(Equipment equipment)
    {
        var allEquipment = GetAll();

        var indexToDelete = allEquipment.FindIndex(e => e.Id == equipment.Id);
        if (indexToDelete == -1) throw new KeyNotFoundException();

        allEquipment.RemoveAt(indexToDelete);

        Serializer<Equipment>.ToCSV(allEquipment, FilePath);
    }
}