using System;
using System.Collections.Generic;
using System.Linq;
using Hospital.Core.Pharmacy.Models;
using Hospital.Serialization;

namespace Hospital.Core.Pharmacy.Repositories;

public class MedicationOrderRepository
{
    private const string FilePath = "../../../Data/medicationOrders.csv";
    private readonly ISerializer<MedicationOrder> _serializer;

    public MedicationOrderRepository(ISerializer<MedicationOrder> serializer)
    {
        _serializer = serializer;
    }

    public List<MedicationOrder> GetAll()
    {
        return _serializer.Load(FilePath);
    }

    public List<MedicationOrder> GetAllExecutable()
    {
        var allMedicationOrders = GetAll();

        return allMedicationOrders.Where(order => (DateTime.Now - order.CreatedDate).TotalDays >= 1).ToList();
    }

    public MedicationOrder? GetById(string id)
    {
        return GetAll().Find(medicationOrder => medicationOrder.Id == id);
    }

    public void Add(MedicationOrder medicationOrder)
    {
        var allMedicationOrders = GetAll();

        allMedicationOrders.Add(medicationOrder);

        _serializer.Save(allMedicationOrders, FilePath);
    }

    public void Update(MedicationOrder medicationOrder)
    {
        var allMedicationOrders = GetAll();

        var indexToUpdate = allMedicationOrders.FindIndex(mo => mo.Id == medicationOrder.Id);
        if (indexToUpdate == -1) throw new KeyNotFoundException();

        allMedicationOrders[indexToUpdate] = medicationOrder;

        _serializer.Save(allMedicationOrders, FilePath);
    }

    public void Delete(MedicationOrder medicationOrder)
    {
        var allMedicationOrders = GetAll();

        var indexToDelete = allMedicationOrders.FindIndex(mo => mo.Id == medicationOrder.Id);
        if (indexToDelete == -1) throw new KeyNotFoundException();

        allMedicationOrders.RemoveAt(indexToDelete);

        _serializer.Save(allMedicationOrders, FilePath);
    }
}