using System.Globalization;
using CsvHelper;
using Hospital.Models.Manager;
using Hospital.Repositories.Manager;

namespace HospitalTests.Repositories.Manager;

[TestClass]
public class EquipmentRepositoryTests

{
    [TestMethod]
    public void TestGetAll()
    {
        var equipment = new List<Equipment>
        {
            new("1", "Chair", Equipment.EquipmentType.FURNITURE),
            new("2", "Operating Table",
                Equipment.EquipmentType.OPERATION_EQUIPMENT),
            new("3", "Stethoscope", Equipment.EquipmentType.EXAMINATION_EQUIPMENT),
            new("4", "Wheelchair", Equipment.EquipmentType.HALLWAY_EQUIPMENT)
        };
        using (var writer = new StreamWriter("../../../Data/equipment.csv"))
        {
            using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csvWriter.WriteRecords(equipment);
            csvWriter.Flush();
        }

        var equipmentRepository = new EquipmentRepository();

        var loadedEquipment = equipmentRepository.GetAll();
        Assert.AreEqual(4, loadedEquipment.Count);
        Assert.AreEqual(Equipment.EquipmentType.OPERATION_EQUIPMENT, loadedEquipment[1].Type);
        Assert.AreEqual("Operating Table", loadedEquipment[1].Name);
    }

    [TestMethod]
    public void TestGetById()
    {
        var equipment = new List<Equipment>
        {
            new("1", "Chair", Equipment.EquipmentType.FURNITURE),
            new("3", "Stethoscope", Equipment.EquipmentType.EXAMINATION_EQUIPMENT),
            new("2", "Operating Table",
                Equipment.EquipmentType.OPERATION_EQUIPMENT),
            new("4", "Wheelchair", Equipment.EquipmentType.HALLWAY_EQUIPMENT)
        };
        using (var writer = new StreamWriter("../../../Data/equipment.csv"))
        {
            using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csvWriter.WriteRecords(equipment);
            csvWriter.Flush();
        }

        var equipmentRepository = new EquipmentRepository();

        Assert.AreEqual("Stethoscope", equipmentRepository.GetById("3").Name);
        Assert.AreEqual(Equipment.EquipmentType.FURNITURE, equipmentRepository.GetById("1").Type);
        Assert.IsNull(equipmentRepository.GetById("0"));
    }

    [TestMethod]
    public void TestUpdate()
    {
        var equipment = new List<Equipment>
        {
            new("1", "Chair", Equipment.EquipmentType.FURNITURE),
            new("2", "Operating Table",
                Equipment.EquipmentType.OPERATION_EQUIPMENT),
            new("3", "Stethoscope", Equipment.EquipmentType.EXAMINATION_EQUIPMENT),
            new("4", "Wheelchair", Equipment.EquipmentType.HALLWAY_EQUIPMENT)
        };
        using (var writer = new StreamWriter("../../../Data/equipment.csv"))
        {
            using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csvWriter.WriteRecords(equipment);
            csvWriter.Flush();
        }

        var equipmentRepository = new EquipmentRepository();

        equipmentRepository.Update(new Equipment("1", "Table", Equipment.EquipmentType.FURNITURE));

        Assert.AreEqual("Table", equipmentRepository.GetById("1").Name);
        Assert.AreEqual(Equipment.EquipmentType.FURNITURE, equipmentRepository.GetById("1").Type);
    }

    [TestMethod]
    public void TestDelete()
    {
        var equipmentToDelete = new Equipment("5", "TO DELETE", Equipment.EquipmentType.EXAMINATION_EQUIPMENT);
        var equipment = new List<Equipment>
        {
            new("1", "Chair", Equipment.EquipmentType.FURNITURE),
            new("2", "Operating Table",
                Equipment.EquipmentType.OPERATION_EQUIPMENT),
            equipmentToDelete,
            new("3", "Stethoscope", Equipment.EquipmentType.EXAMINATION_EQUIPMENT),
            new("4", "Wheelchair", Equipment.EquipmentType.HALLWAY_EQUIPMENT)
        };
        using (var writer = new StreamWriter("../../../Data/equipment.csv"))
        {
            using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csvWriter.WriteRecords(equipment);
            csvWriter.Flush();
        }

        var equipmentRepository = new EquipmentRepository();

        equipmentRepository.Delete(equipmentToDelete);

        Assert.AreEqual(4, equipmentRepository.GetAll().Count);
        Assert.IsNull(equipmentRepository.GetById("5"));
    }

    [TestMethod]
    public void TestAdd()
    {
        var newEquipment = new Equipment("5", "C-Arm", Equipment.EquipmentType.OPERATION_EQUIPMENT);
        var equipment = new List<Equipment>
        {
            new("1", "Chair", Equipment.EquipmentType.FURNITURE),
            new("2", "Operating Table",
                Equipment.EquipmentType.OPERATION_EQUIPMENT),
            new("3", "Stethoscope", Equipment.EquipmentType.EXAMINATION_EQUIPMENT),
            new("4", "Wheelchair", Equipment.EquipmentType.HALLWAY_EQUIPMENT)
        };
        using (var writer = new StreamWriter("../../../Data/equipment.csv"))
        {
            using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csvWriter.WriteRecords(equipment);
            csvWriter.Flush();
        }

        var equipmentRepository = new EquipmentRepository();

        equipmentRepository.Add(newEquipment);
        var loadedNewEquipment = equipmentRepository.GetById("5");

        Assert.AreEqual(5, equipmentRepository.GetAll().Count);
        Assert.AreEqual("C-Arm", loadedNewEquipment.Name);
        Assert.AreEqual(Equipment.EquipmentType.OPERATION_EQUIPMENT, loadedNewEquipment.Type);
    }

    [TestMethod]
    public void TestUpdateNonExistentEquipment()
    {
        var equipment = new List<Equipment>
        {
            new("1", "Chair", Equipment.EquipmentType.FURNITURE),
            new("2", "Operating Table",
                Equipment.EquipmentType.OPERATION_EQUIPMENT),
            new("3", "Stethoscope", Equipment.EquipmentType.EXAMINATION_EQUIPMENT),
            new("4", "Wheelchair", Equipment.EquipmentType.HALLWAY_EQUIPMENT)
        };
        using (var writer = new StreamWriter("../../../Data/equipment.csv"))
        {
            using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csvWriter.WriteRecords(equipment);
            csvWriter.Flush();
        }

        var equipmentRepository = new EquipmentRepository();

        Assert.ThrowsException<KeyNotFoundException>(() => equipmentRepository.Update(new Equipment("0", "Nonexistent",
            Equipment.EquipmentType.FURNITURE)));
    }

    [TestMethod]
    public void TestDeleteNonExistentEquipment()
    {
        var equipment = new List<Equipment>
        {
            new("1", "Chair", Equipment.EquipmentType.FURNITURE),
            new("2", "Operating Table",
                Equipment.EquipmentType.OPERATION_EQUIPMENT),
            new("3", "Stethoscope", Equipment.EquipmentType.EXAMINATION_EQUIPMENT),
            new("4", "Wheelchair", Equipment.EquipmentType.HALLWAY_EQUIPMENT)
        };
        using (var writer = new StreamWriter("../../../Data/equipment.csv"))
        {
            using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csvWriter.WriteRecords(equipment);
            csvWriter.Flush();
        }

        var equipmentRepository = new EquipmentRepository();

        Assert.ThrowsException<KeyNotFoundException>(() => equipmentRepository.Delete(new Equipment("0", "Nonexistent",
            Equipment.EquipmentType.FURNITURE)));
    }
}