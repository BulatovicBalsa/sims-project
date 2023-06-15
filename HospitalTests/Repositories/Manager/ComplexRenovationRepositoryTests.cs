using Hospital.Core.PhysicalAssets.Models;
using Hospital.Core.PhysicalAssets.Repositories;
using Hospital.Core.Scheduling;
using Hospital.Injectors;
using Hospital.Serialization;

namespace HospitalTests.Repositories.Manager;

[TestClass]
public class ComplexRenovationRepositoryTests
{
    [TestInitialize]
    public void SetUp()
    {
        try
        {
            DeleteData();
        }
        catch (Exception)
        {
            Console.WriteLine("Files don't exist.");
        }

        new ComplexRenovationRepository(SerializerInjector.CreateInstance<ISerializer<ComplexRenovation>>()).GetAllFromFile();
    }

    [TestCleanup]
    public void CleanUp()
    {
        try
        {
            DeleteData();
        }
        catch (Exception)
        {
            Console.WriteLine("Files don't exist.");
        }
    }

    private static void DeleteData()
    {
        Directory.GetFiles("../../../Data/").ToList().ForEach(File.Delete);
    }

    [TestMethod]
    public void TestAdd()
    {
        var toDemolish = new List<Room>
        {
            new("Examination room", RoomType.ExaminationRoom),
            new("Ward", RoomType.Ward)
        };
        var toBuild = new List<Room>
        {
            new("Operating room", RoomType.OperatingRoom)
        };

        RoomRepository.Instance.Add(toDemolish);
        RoomRepository.Instance.Add(toBuild);

        var equipment = new Equipment("Something", EquipmentType.DynamicEquipment);
        var otherEquipment = new Equipment("Other", EquipmentType.DynamicEquipment);
        toDemolish[0].SetAmount(equipment, 10);
        toDemolish[1].SetAmount(otherEquipment, 3);
        var fromOldToNew = new Transfer(toDemolish[0], toBuild[0], DateTime.Now);
        fromOldToNew.AddItem(new TransferItem(equipment, 8));

        var complexRenovation = new ComplexRenovation(toDemolish, toBuild,
            new TimeRange(DateTime.Now.AddDays(-1), DateTime.Now),
            toBuild[0], new List<Transfer> { fromOldToNew });
        var complexRenovationRepository =
            new ComplexRenovationRepository(SerializerInjector.CreateInstance<ISerializer<ComplexRenovation>>());

        var complexRenovations = new List<ComplexRenovation>
        {
            complexRenovation
        };
        complexRenovationRepository.Add(complexRenovations);

        Assert.AreEqual(complexRenovations.Count, complexRenovationRepository.GetAllFromFile().Count);
    }

    [TestMethod]
    public void TestGetAll()
    {
        var toDemolish = new List<Room>
        {
            new("Examination room", RoomType.ExaminationRoom),
            new("Ward", RoomType.Ward)
        };
        var toBuild = new List<Room>
        {
            new("Operating room", RoomType.OperatingRoom)
        };

        RoomRepository.Instance.Add(toDemolish);
        RoomRepository.Instance.Add(toBuild);

        var equipment = new Equipment("Something", EquipmentType.DynamicEquipment);
        var otherEquipment = new Equipment("Other", EquipmentType.DynamicEquipment);
        toDemolish[0].SetAmount(equipment, 10);
        toDemolish[1].SetAmount(otherEquipment, 3);
        var fromOldToNew = new Transfer(toDemolish[0], toBuild[0], DateTime.Now);
        fromOldToNew.AddItem(new TransferItem(equipment, 8));

        var complexRenovation = new ComplexRenovation(toDemolish, toBuild,
            new TimeRange(DateTime.Now.AddDays(-1), DateTime.Now),
            toBuild[0], new List<Transfer> { fromOldToNew });
        var complexRenovationRepository =
            new ComplexRenovationRepository(SerializerInjector.CreateInstance<ISerializer<ComplexRenovation>>());

        var complexRenovations = new List<ComplexRenovation>
        {
            complexRenovation
        };
        complexRenovationRepository.Add(complexRenovations);

        var complexRenovationsFromFile = complexRenovationRepository.GetAllFromFile();
        Assert.AreEqual(complexRenovations.Count, complexRenovationsFromFile.Count);

        Assert.AreSame(complexRenovationsFromFile[0].TransfersFromOldToNewRooms[0].Origin, toDemolish[0],
            "References were not reestablished correctly");
        Assert.AreSame(complexRenovationsFromFile[0].TransfersFromOldToNewRooms[0].Destination, toBuild[0],
            "References were not reestablished correctly");
        Assert.AreSame(complexRenovationsFromFile[0].LeftoverEquipmentDestination, toBuild[0]);


        for (var i = 0; i < toBuild.Count; i++) Assert.AreSame(toBuild[i], complexRenovationsFromFile[0].ToBuild[i]);
        for (var i = 0; i < toDemolish.Count; i++)
            Assert.AreSame(toDemolish[i], complexRenovationsFromFile[0].ToDemolish[i]);
    }

    [TestMethod]
    public void TestUpdate()
    {
        var toDemolish = new List<Room>
        {
            new("Examination room", RoomType.ExaminationRoom),
            new("Ward", RoomType.Ward)
        };
        var toBuild = new List<Room>
        {
            new("Operating room", RoomType.OperatingRoom)
        };

        RoomRepository.Instance.Add(toDemolish);
        RoomRepository.Instance.Add(toBuild);

        var equipment = new Equipment("Something", EquipmentType.DynamicEquipment);
        var otherEquipment = new Equipment("Other", EquipmentType.DynamicEquipment);
        toDemolish[0].SetAmount(equipment, 10);
        toDemolish[1].SetAmount(otherEquipment, 3);
        var fromOldToNew = new Transfer(toDemolish[0], toBuild[0], DateTime.Now);
        fromOldToNew.AddItem(new TransferItem(equipment, 8));

        var complexRenovation = new ComplexRenovation(toDemolish, toBuild,
            new TimeRange(DateTime.Now.AddDays(-1), DateTime.Now.AddMinutes(-1)),
            toBuild[0], new List<Transfer> { fromOldToNew });
        var complexRenovationRepository =
            new ComplexRenovationRepository(SerializerInjector.CreateInstance<ISerializer<ComplexRenovation>>());

        var complexRenovations = new List<ComplexRenovation>
        {
            complexRenovation
        };
        complexRenovationRepository.Add(complexRenovations);
        complexRenovation.Schedule();
        Assert.IsTrue(complexRenovation.TryComplete());

        complexRenovationRepository.Update(complexRenovation);
        var complexRenovationsFromFile = complexRenovationRepository.GetAllFromFile();
        Assert.IsTrue(complexRenovationsFromFile[0].Completed);
    }
}