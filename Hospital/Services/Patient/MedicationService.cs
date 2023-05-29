using System.Collections.Generic;
using Hospital.Models.Patient;
using Hospital.Repositories.Patient;

namespace Hospital.Services;
public class MedicationService
{
    private readonly MedicationRepository _medicationRepository;

    public MedicationService()
    {
        _medicationRepository = MedicationRepository.Instance;
    }

    public int GetMedicationStock(Medication medication)
    {
        var requestedMedication = _medicationRepository.GetById(medication.Id);

        return requestedMedication?.Stock ?? 0;
    }

    public void DecrementMedicationStock(Medication medication)
    {
        medication.Stock--;
        _medicationRepository.Update(medication);
    }

    public List<Medication> GetLowStockMedication()
    {
        return _medicationRepository.GetLowStockMedication();
    }
}
