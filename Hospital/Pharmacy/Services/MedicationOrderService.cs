using System;
using Hospital.DTOs;
using Hospital.Injectors;
using Hospital.Pharmacy.Models;
using Hospital.Pharmacy.Repositories;
using Hospital.Serialization;

namespace Hospital.Pharmacy.Services;

public class MedicationOrderService
{
    private readonly MedicationOrderRepository _medicationOrderRepository;
    private readonly MedicationService _medicationService;

    public MedicationOrderService()
    {
        _medicationOrderRepository =
            new MedicationOrderRepository(SerializerInjector.CreateInstance<ISerializer<MedicationOrder>>());
        _medicationService = new MedicationService();
    }

    public void AddNewOrder(MedicationOrderQuantityDto medicationOrderQuantityDto)
    {
        var medicationOrder = new MedicationOrder
        {
            MedicationId = medicationOrderQuantityDto.MedicationId,
            Quantity = medicationOrderQuantityDto.OrderQuantity,
            CreatedDate = DateTime.Now
        };

        _medicationOrderRepository.Add(medicationOrder);
    }

    public void ExecuteMedicationOrders()
    {
        var executableOrders = _medicationOrderRepository.GetAllExecutable();

        foreach (var executableOrder in executableOrders)
        {
            _medicationService.IncrementMedicationStock(executableOrder.MedicationId, executableOrder.Quantity);
            _medicationOrderRepository.Delete(executableOrder);
        }
    }
}