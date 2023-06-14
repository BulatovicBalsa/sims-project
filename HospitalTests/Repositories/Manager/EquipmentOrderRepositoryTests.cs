using Hospital.Models.Manager;
using Hospital.Repositories.Manager;
using Hospital.Serialization;

namespace HospitalTests.Services.Manager;

[TestClass]
public class EquipmentOrderRepositoryTests
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

        EquipmentOrderRepository.Instance.DeleteAll();
        var orders = new List<EquipmentOrder>
        {
            new("1", new DateTime(), false),
            new("2", new DateTime(), false)
        };

        CsvSerializer<EquipmentOrder>.ToCSV(orders, "../../../Data/equipmentOrders.csv");

        var orderItems = new List<EquipmentOrderItem>
        {
            new("1", 10, "1"),
            new("1", 20, "2"),
            new("2", 1, "3")
        };

        CsvSerializer<EquipmentOrderItem>.ToCSV(orderItems, "../../../Data/equipmentOrderItems.csv");
    }
    private static void DeleteData()
    {
        Directory.GetFiles("../../../Data/").ToList().ForEach(File.Delete);
    }


    [TestMethod]
    public void TestGetAll()
    {
        var equipmentOrderRepository = EquipmentOrderRepository.Instance;
        var orders = equipmentOrderRepository.GetAll();
        Assert.AreEqual(2, orders.Count);
        Assert.AreEqual(2, orders[0].Items.Count);
    }

    [TestMethod]
    public void TestAdd()
    {
        var equipmentOrderRepository = EquipmentOrderRepository.Instance;
        var newOrder = new EquipmentOrder(DateTime.Now);
        newOrder.AddOrUpdateItem("1", 4);
        equipmentOrderRepository.Add(newOrder);
        var orders = equipmentOrderRepository.GetAll();
        Assert.AreEqual(3, orders.Count);
        Assert.AreEqual(1, orders[2].Items.Count);
        Assert.AreEqual(4, orders[2].Items[0].Amount);
    }

    [TestMethod]
    public void TestUpdate()
    {
        var equipmentOrderRepository = EquipmentOrderRepository.Instance;
        var orderToChange = equipmentOrderRepository.GetAll()[0];
        orderToChange.AddOrUpdateItem("4", 1);
        equipmentOrderRepository.Update(orderToChange);

        var changedOrder = equipmentOrderRepository.GetAll()[0];
        Assert.AreEqual(3, changedOrder.Items.Count);
    }
}