using Hospital.PhysicalAssets.Models;

namespace HospitalTests.Models.Manager;

[TestClass]
public class RenovationTests
{
    [TestMethod]
    public void TestTryComplete()
    {
        var room = new Room("143", "Room", RoomType.WaitingRoom);
        var renovation = new Renovation("143", DateTime.Now.AddDays(-1), DateTime.Now.AddSeconds(-5), room);
        Assert.IsTrue(renovation.TryComplete());
    }

    [TestMethod]
    public void TestTryCompleteNotCompleted()
    {
        var room = new Room("143", "Room", RoomType.WaitingRoom);
        var renovation = new Renovation("143", DateTime.Now.AddDays(-1), DateTime.Now.AddSeconds(1000), room);
        Assert.IsFalse(renovation.TryComplete());
    }

    [TestMethod]
    public void TestOverlapsWithDayOverlaps()
    {
        var room = new Room("143", "Room", RoomType.WaitingRoom);
        var renovation = new Renovation("143", DateTime.Now.AddDays(-1), DateTime.Now.AddSeconds(1000), room);

        Assert.IsTrue(renovation.OverlapsWith(DateTime.Now.AddHours(-2)));
    }

    [TestMethod]
    public void TestOverlapsWithIntervalWiderThanRenovation()
    {
        var room = new Room("143", "Room", RoomType.WaitingRoom);
        var renovation = new Renovation("143", DateTime.Now.AddDays(-1), DateTime.Now.AddSeconds(1000), room);

        Assert.IsTrue(renovation.OverlapsWith(DateTime.Now.AddDays(-2), DateTime.Now.AddDays(4)));
    }

    [TestMethod]
    public void TestOverlapsWithIntervalSmallerThanRenovation()
    {
        var room = new Room("143", "Room", RoomType.WaitingRoom);
        var renovation = new Renovation("143", DateTime.Now.AddDays(-1), DateTime.Now.AddSeconds(1000), room);

        Assert.IsTrue(renovation.OverlapsWith(DateTime.Now.AddHours(-5), DateTime.Now.AddHours(-2)));
    }

    [TestMethod]
    public void TestOverlapsWithIntervalDoesNotOverlap()
    {
        var room = new Room("143", "Room", RoomType.WaitingRoom);
        var renovation = new Renovation("143", DateTime.Now.AddDays(-1), DateTime.Now.AddSeconds(1000), room);

        Assert.IsFalse(renovation.OverlapsWith(DateTime.Now.AddHours(5), DateTime.Now.AddHours(20)));
    }
}