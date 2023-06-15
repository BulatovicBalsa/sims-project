using Hospital.PhysicalAssets.Models;
using Hospital.PhysicalAssets.Repositories;

namespace HospitalTests.Repositories.Manager;

[TestClass]
public class RenovationRepositoryTests
{
    public const string roomFilePath = "../../../Data/rooms.csv";
    public const string renovationFilePath = "../../../Data/renovations.csv";

    [TestInitialize]
    public void SetUp()
    {
        if (File.Exists(roomFilePath)) File.Delete(roomFilePath);

        if (File.Exists(renovationFilePath)) File.Delete(renovationFilePath);

        RoomRepository.Instance.Add(new Room("1", "Warehouse", RoomType.Warehouse));
        RoomRepository.Instance.Add(new Room("2", "Ward", RoomType.Ward));
        RenovationRepository.Instance.GetAllFromFile();
    }

    [TestCleanup]
    public void CleanUp()
    {
        if (File.Exists(roomFilePath))
            File.Delete(roomFilePath);
        if (File.Exists(renovationFilePath))
            File.Delete(renovationFilePath);
    }

    [TestMethod]
    public void TestGetAll()
    {
        File.WriteAllText(renovationFilePath,
            "Id,RoomId,BeginTime,EndTime,Completed,Id,Name,Type,IsDemolished,CreationDate,DemolitionDate\r\nb98e755f-0b31-4167-a851-84adabdb034c,1,05/28/2023 02:53:15,05/28/2023 02:53:15,False,1,Warehouse,Warehouse,False,,\r\n41cf0257-2048-4ffa-a72a-9b23fb83b3a7,2,05/28/2023 02:53:15,05/28/2023 02:53:15,False,2,Ward,Ward,False,,");
        Assert.AreEqual(2, RenovationRepository.Instance.GetAllFromFile().Count);
    }

    [TestMethod]
    public void TestAdd()
    {
        RenovationRepository.Instance.Add(new Renovation("1", DateTime.Now, DateTime.Now,
            RoomRepository.Instance.GetById("1") ?? throw new InvalidOperationException()));
        RenovationRepository.Instance.Add(new Renovation("2", DateTime.Now, DateTime.Now,
            RoomRepository.Instance.GetById("2") ?? throw new InvalidOperationException()));
        Assert.AreEqual(2, RenovationRepository.Instance.GetAllFromFile().Count);
    }

    [TestMethod]
    public void TestUpdate()
    {
        var renovation = new Renovation("1", DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-1),
            RoomRepository.Instance.GetById("1") ?? throw new InvalidOperationException());
        RenovationRepository.Instance.Add(renovation);
        Assert.IsTrue(renovation.TryComplete(), "Renovation to update did not complete");
        RenovationRepository.Instance.Update(renovation);
        Assert.AreEqual(1, RenovationRepository.Instance.GetAllFromFile().Count);
        Assert.IsTrue(RenovationRepository.Instance.GetAllFromFile()[0].Completed);
    }
}