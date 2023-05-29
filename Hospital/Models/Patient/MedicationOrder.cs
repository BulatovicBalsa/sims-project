using System;

namespace Hospital.Models.Patient;
public class MedicationOrder
{
    public string Id { get; set; }
    public string MedicationId { get; set; }
    public int Quantity { get; set; }
    public DateTime CreatedDate { get; set; }

    public MedicationOrder()
    {
        Id = Guid.NewGuid().ToString();
    }
}
