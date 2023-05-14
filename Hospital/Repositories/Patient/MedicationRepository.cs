using Hospital.Serialization;
using System.Collections.Generic;
using Hospital.Models.Patient;

namespace Hospital.Repositories.Patient;
public class MedicationRepository
{
    private const string FilePath = "../../../Data/Medications.csv";
    private static MedicationRepository? _instance;

    public static MedicationRepository Instance => _instance ??= new MedicationRepository();

    private MedicationRepository() { }

    public List<Medication> GetAll()
    {
        return Serializer<Medication>.FromCSV(FilePath);
    }

    public Medication? GetById(string id)
    {
        return GetAll().Find(medication => medication.Id == id);
    }

    public void Add(Medication medication)
    {
        var allMedication = GetAll();

        allMedication.Add(medication);

        Serializer<Medication>.ToCSV(allMedication, FilePath);
    }

    public void Update(Medication medication)
    {
        var allMedication = GetAll();

        var indexToUpdate = allMedication.FindIndex(e => e.Id == medication.Id);
        if (indexToUpdate == -1) throw new KeyNotFoundException();

        allMedication[indexToUpdate] = medication;

        Serializer<Medication>.ToCSV(allMedication, FilePath);
    }

    public void Delete(Medication medication)
    {
        var allMedication = GetAll();

        var indexToDelete = allMedication.FindIndex(e => e.Id == medication.Id);
        if (indexToDelete == -1) throw new KeyNotFoundException();

        allMedication.RemoveAt(indexToDelete);

        Serializer<Medication>.ToCSV(allMedication, FilePath);
    }

    public static void DeleteAll()
    {
        var emptyMedicationList = new List<Medication>();
        Serializer<Medication>.ToCSV(emptyMedicationList, FilePath);
    }
}