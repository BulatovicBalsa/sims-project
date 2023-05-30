﻿namespace Hospital.DTOs;
public class MedicationOrderQuantityDto
{
    public string MedicationId { get; set; }
    public string Name { get; set; }
    public int Stock { get; set; }
    public int OrderQuantity { get; set; }

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
}
