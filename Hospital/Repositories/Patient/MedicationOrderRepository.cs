using Hospital.Models.Patient;
using Hospital.Serialization;
using System.Collections.Generic;

namespace Hospital.Repositories.Patient;
public class MedicationOrderRepository
{
    private const string FilePath = "../../../Data/medicationOrders.csv";
    private static MedicationOrderRepository? _instance;

    public static MedicationOrderRepository Instance => _instance ??= new MedicationOrderRepository();

    private MedicationOrderRepository() { }

    public List<MedicationOrder> GetAll()
    {
        return Serializer<MedicationOrder>.FromCSV(FilePath);
    }

    public MedicationOrder? GetById(string id)
    {
        return GetAll().Find(medicationOrder => medicationOrder.Id == id);
    }

    public void Add(MedicationOrder medicationOrder)
    {
        var allMedicationOrders = GetAll();

        allMedicationOrders.Add(medicationOrder);

        Serializer<MedicationOrder>.ToCSV(allMedicationOrders, FilePath);
    }

    public void Update(MedicationOrder medicationOrder)
    {
        var allMedicationOrders = GetAll();

        var indexToUpdate = allMedicationOrders.FindIndex(mo => mo.Id == medicationOrder.Id);
        if (indexToUpdate == -1) throw new KeyNotFoundException();

        allMedicationOrders[indexToUpdate] = medicationOrder;

        Serializer<MedicationOrder>.ToCSV(allMedicationOrders, FilePath);
    }

    public void Delete(MedicationOrder medicationOrder)
    {
        var allMedicationOrders = GetAll();

        var indexToDelete = allMedicationOrders.FindIndex(mo => mo.Id == medicationOrder.Id);
        if (indexToDelete == -1) throw new KeyNotFoundException();

        allMedicationOrders.RemoveAt(indexToDelete);

        Serializer<MedicationOrder>.ToCSV(allMedicationOrders, FilePath);
    }
}