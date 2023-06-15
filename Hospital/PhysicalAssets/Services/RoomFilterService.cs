using System.Collections.Generic;
using System.Linq;
using Hospital.PhysicalAssets.Models;
using Hospital.PhysicalAssets.Repositories;

namespace Hospital.PhysicalAssets.Services;

public class RoomFilterService
{
    private const int lowEquipmentThreshold = 5;
    private readonly RoomRepository _roomRepository = RoomRepository.Instance;

    public static List<Room> GetRoomsLowOnDynamicEquipment()
    {
        var dynamicEquipment = EquipmentRepository.Instance.GetDynamic();
        var rooms = RoomRepository.Instance.GetAll();
        return rooms.Where(room =>
            dynamicEquipment.Exists(equipment => room.GetAmount(equipment) < lowEquipmentThreshold)).ToList();
    }

    public List<Room> GetRoomsForExamination()
    {
        var allRooms = RoomRepository.Instance.GetAll();
        return allRooms.Where(room =>
            room.Type is RoomType.OperatingRoom or RoomType.ExaminationRoom).ToList();
    }

    public List<Room> GetRoomsForAccommodation()
    {
        return _roomRepository.GetAll(RoomType.Ward);
    }
}