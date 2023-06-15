using System;

namespace Hospital.Pharmacy.Models;

public class MedicationOrder
{
    public MedicationOrder()
    {
        Id = Guid.NewGuid().ToString();
    }

    public string Id { get; set; }
    public string MedicationId { get; set; }
    public int Quantity { get; set; }
    public DateTime CreatedDate { get; set; }
}