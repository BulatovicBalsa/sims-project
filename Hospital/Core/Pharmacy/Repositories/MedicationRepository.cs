using System.Collections.Generic;
using System.Linq;
using Hospital.Core.Pharmacy.Models;
using Hospital.Serialization;
using Hospital.Serialization.Mappers.Medication;

namespace Hospital.Core.Pharmacy.Repositories;

public class MedicationRepository
{
    private const string FilePath = "../../../Data/medications.csv";
    private static MedicationRepository? _instance;

    private MedicationRepository()
    {
    }

    public static MedicationRepository Instance => _instance ??= new MedicationRepository();

    public List<Medication> GetAll()
    {
        return CsvSerializer<Medication>.FromCSV(FilePath, new MedicationReadMapper());
    }

    public Medication? GetById(string id)
    {
        return GetAll().Find(medication => medication.Id == id);
    }

    public void Add(Medication medication)
    {
        var allMedication = GetAll();

        allMedication.Add(medication);

        CsvSerializer<Medication>.ToCSV(allMedication, FilePath, new MedicationWriteMapper());
    }

    public void Update(Medication medication)
    {
        var allMedication = GetAll();

        var indexToUpdate = allMedication.FindIndex(e => e.Id == medication.Id);
        if (indexToUpdate == -1) throw new KeyNotFoundException();

        allMedication[indexToUpdate] = medication;

        CsvSerializer<Medication>.ToCSV(allMedication, FilePath, new MedicationWriteMapper());
    }

    public void Delete(Medication medication)
    {
        var allMedication = GetAll();

        var indexToDelete = allMedication.FindIndex(e => e.Id == medication.Id);
        if (indexToDelete == -1) throw new KeyNotFoundException();

        allMedication.RemoveAt(indexToDelete);

        CsvSerializer<Medication>.ToCSV(allMedication, FilePath, new MedicationWriteMapper());
    }

    public static void DeleteAll()
    {
        var emptyMedicationList = new List<Medication>();
        CsvSerializer<Medication>.ToCSV(emptyMedicationList, FilePath, new MedicationWriteMapper());
    }

    public List<Medication> GetLowStockMedication()
    {
        var allMedication = GetAll();
        return allMedication.Where(medication => medication.Stock < 5).ToList();
    }
}