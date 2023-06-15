namespace Hospital.Core.Pharmacy.DTOs;

public class MedicationOrderQuantityDto
{
    public MedicationOrderQuantityDto()
    {
    }

    public MedicationOrderQuantityDto(string medicationId, string name, int stock, int orderQuantity)
    {
        MedicationId = medicationId;
        Name = name;
        Stock = stock;
        OrderQuantity = orderQuantity;
    }

    public string MedicationId { get; set; }
    public string Name { get; set; }
    public int Stock { get; set; }
    public int OrderQuantity { get; set; }
}