using System;
using Hospital.Models.Patient;
using Hospital.Repositories.Patient;
using Hospital.ViewModels.Nurse.Medication;

namespace Hospital.Services;
public class MedicationOrderService
{
    private readonly MedicationOrderRepository _medicationOrderRepository;

    public MedicationOrderService()
    {
        _medicationOrderRepository = MedicationOrderRepository.Instance;
    }

    public void AddNewOrder(MedicationOrderQuantityDto medicationOrderQuantityDto)
    {
        var medicationOrder = new MedicationOrder()
        {
            MedicationId = medicationOrderQuantityDto.MedicationId,
            Quantity = medicationOrderQuantityDto.OrderQuantity,
            CreatedDate = DateTime.Now
        };

        _medicationOrderRepository.Add(medicationOrder);
    }
}
