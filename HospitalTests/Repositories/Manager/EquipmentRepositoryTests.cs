using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using System.Windows.Documents;
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
        //TODO: Fix arrange code duplication
        var equipment = new List<Equipment>
        {
            new(1, "Chair", Equipment.EquipmentType.FURNITURE),
            new(2, "Operating Table",
                Equipment.EquipmentType.OPERATION_EQUIPMENT),
            new(3, "Stethoscope", Equipment.EquipmentType.EXAMINATION_EQUIPMENT),
            new(4, "Wheelchair", Equipment.EquipmentType.HALLWAY_EQUIPMENT)
        };
        using (var writer = new StreamWriter("../../../Data/equipment.csv"))
        {
            using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csvWriter.WriteRecords(equipment);
            csvWriter.Flush();
        }

        var equipmentRepository = new EquipmentRepository();

        var loadedEquipment = equipmentRepository.GetAll();
        Assert.AreEqual(loadedEquipment.Count, 4);
        Assert.AreEqual(equipment[3].Type, Equipment.EquipmentType.HALLWAY_EQUIPMENT);
        Assert.AreEqual(equipment[1].Name, "Operating Table");
    }

    [TestMethod]
    public void TestGetById()
    {
        var equipment = new List<Equipment>
        {
            new(1, "Chair", Equipment.EquipmentType.FURNITURE),
            new(3, "Stethoscope", Equipment.EquipmentType.EXAMINATION_EQUIPMENT),
            new(2, "Operating Table",
                Equipment.EquipmentType.OPERATION_EQUIPMENT),
            new(4, "Wheelchair", Equipment.EquipmentType.HALLWAY_EQUIPMENT)
        };
        using (var writer = new StreamWriter("../../../Data/equipment.csv"))
        {
            using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csvWriter.WriteRecords(equipment);
            csvWriter.Flush();
        }

        var equipmentRepository = new EquipmentRepository();

        Assert.AreEqual(equipmentRepository.GetById(3).Name, "Stethoscope");
        Assert.AreEqual(equipmentRepository.GetById(1).Type, Equipment.EquipmentType.FURNITURE);
        Assert.IsNull(equipmentRepository.GetById(0));


    }

    [TestMethod]
    public void TestUpdate()
    {
        var equipment = new List<Equipment>
        {
            new(1, "Chair", Equipment.EquipmentType.FURNITURE),
            new(2, "Operating Table",
                Equipment.EquipmentType.OPERATION_EQUIPMENT),
            new(3, "Stethoscope", Equipment.EquipmentType.EXAMINATION_EQUIPMENT),
            new(4, "Wheelchair", Equipment.EquipmentType.HALLWAY_EQUIPMENT)
        };
        using (var writer = new StreamWriter("../../../Data/equipment.csv"))
        {
            using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csvWriter.WriteRecords(equipment);
            csvWriter.Flush();
        }

        var equipmentRepository = new EquipmentRepository();

        equipmentRepository.Update(new(1, "Table", Equipment.EquipmentType.FURNITURE));

        Assert.AreEqual(equipmentRepository.GetById(1).Name, "Table");
        Assert.AreEqual(equipmentRepository.GetById(1).Type, Equipment.EquipmentType.FURNITURE);
    }

    [TestMethod]
    public void TestDelete()
    {
        var equipmentToDelete = new Equipment(5, "TO DELETE", Equipment.EquipmentType.EXAMINATION_EQUIPMENT);
        var equipment = new List<Equipment>
        {
            new(1, "Chair", Equipment.EquipmentType.FURNITURE),
            new(2, "Operating Table",
                Equipment.EquipmentType.OPERATION_EQUIPMENT),
            equipmentToDelete,
            new(3, "Stethoscope", Equipment.EquipmentType.EXAMINATION_EQUIPMENT),
            new(4, "Wheelchair", Equipment.EquipmentType.HALLWAY_EQUIPMENT)
        };
        using (var writer = new StreamWriter("../../../Data/equipment.csv"))
        {
            using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csvWriter.WriteRecords(equipment);
            csvWriter.Flush();
        }

        var equipmentRepository = new EquipmentRepository();

        equipmentRepository.Delete(equipmentToDelete);

        Assert.AreEqual(equipmentRepository.GetAll().Count, 4);
        Assert.IsNull(equipmentRepository.GetById(5));

    }

    [TestMethod()]
    public void TestAdd()
    {
        var newEquipment = new Equipment(5, "C-Arm", Equipment.EquipmentType.OPERATION_EQUIPMENT);
        var equipment = new List<Equipment>
        {
            new(1, "Chair", Equipment.EquipmentType.FURNITURE),
            new(2, "Operating Table",
                Equipment.EquipmentType.OPERATION_EQUIPMENT),
            new(3, "Stethoscope", Equipment.EquipmentType.EXAMINATION_EQUIPMENT),
            new(4, "Wheelchair", Equipment.EquipmentType.HALLWAY_EQUIPMENT)
        };
        using (var writer = new StreamWriter("../../../Data/equipment.csv"))
        {
            using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csvWriter.WriteRecords(equipment);
            csvWriter.Flush();
        }

        var equipmentRepository = new EquipmentRepository();

        equipmentRepository.Add(newEquipment);

        Assert.AreEqual(equipmentRepository.GetAll().Count, 5);
        var loadedNewEquipment = equipmentRepository.GetById(5);
        Assert.AreEqual(loadedNewEquipment.Name, "C-Arm");
        Assert.AreEqual(loadedNewEquipment.Type, Equipment.EquipmentType.OPERATION_EQUIPMENT);
    }

    [TestMethod]
    public void TestUpdateNonExistentEquipment()
    {
        var equipment = new List<Equipment>
        {
            new(1, "Chair", Equipment.EquipmentType.FURNITURE),
            new(2, "Operating Table",
                Equipment.EquipmentType.OPERATION_EQUIPMENT),
            new(3, "Stethoscope", Equipment.EquipmentType.EXAMINATION_EQUIPMENT),
            new(4, "Wheelchair", Equipment.EquipmentType.HALLWAY_EQUIPMENT)
        };
        using (var writer = new StreamWriter("../../../Data/equipment.csv"))
        {
            using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csvWriter.WriteRecords(equipment);
            csvWriter.Flush();
        }

        var equipmentRepository = new EquipmentRepository();

        Assert.ThrowsException<KeyNotFoundException>(() => equipmentRepository.Update(new Equipment(0, "Nonexistent",
            Equipment.EquipmentType.FURNITURE)));
    }

    [TestMethod]
    public void TestDeleteNonExistentEquipment()
    {
        var equipment = new List<Equipment>
        {
            new(1, "Chair", Equipment.EquipmentType.FURNITURE),
            new(2, "Operating Table",
                Equipment.EquipmentType.OPERATION_EQUIPMENT),
            new(3, "Stethoscope", Equipment.EquipmentType.EXAMINATION_EQUIPMENT),
            new(4, "Wheelchair", Equipment.EquipmentType.HALLWAY_EQUIPMENT)
        };
        using (var writer = new StreamWriter("../../../Data/equipment.csv"))
        {
            using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csvWriter.WriteRecords(equipment);
            csvWriter.Flush();
        }

        var equipmentRepository = new EquipmentRepository();

        Assert.ThrowsException<KeyNotFoundException>(() => equipmentRepository.Delete(new Equipment(0, "Nonexistent",
            Equipment.EquipmentType.FURNITURE)));
    }
}