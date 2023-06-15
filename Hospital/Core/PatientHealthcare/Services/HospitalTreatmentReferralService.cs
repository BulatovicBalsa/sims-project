using System.Collections.Generic;
using System.Linq;
using Hospital.Core.PatientHealthcare.Models;
using Hospital.Core.PatientHealthcare.Repositories;
using Hospital.Core.PhysicalAssets.Models;
using Hospital.Core.PhysicalAssets.Repositories;
using Hospital.Core.Scheduling.Services;

namespace Hospital.Core.PatientHealthcare.Services;

public class HospitalTreatmentReferralService
{
    private readonly PatientRepository _patientRepository;
    private readonly RoomRepository _roomRepository;
    private readonly RoomScheduleService _roomScheduleService;

    public HospitalTreatmentReferralService()
    {
        _patientRepository = PatientRepository.Instance;
        _roomRepository = RoomRepository.Instance;
        _roomScheduleService = new RoomScheduleService();
    }

    public List<HospitalTreatmentReferral> GetActiveHospitalTreatmentReferrals(string roomId)
    {
        var allPatients = _patientRepository.GetAll();
        var activeHospitalTreatmentReferrals =
            allPatients.Select(patient => patient.GetActiveHospitalTreatmentReferral());

        return activeHospitalTreatmentReferrals.Where(referral => referral != null && referral.RoomId == roomId)
            .ToList()!;
    }

    public Dictionary<Room, int> GetRoomPatientCounts()
    {
        var wards = _roomRepository.GetAll(RoomType.Ward);
        var roomPatientCounts = new Dictionary<Room, int>();

        foreach (var ward in wards)
        {
            var roomPatientsCount = GetActiveHospitalTreatmentReferrals(ward.Id).Count();
            roomPatientCounts.Add(ward, roomPatientsCount);
        }

        return roomPatientCounts;
    }

    public List<Room> GetAvailableRooms()
    {
        var roomPatientCounts = GetRoomPatientCounts();
        var availableRooms = new List<Room>();

        foreach (var (room, patientCount) in roomPatientCounts)
            if (patientCount < 3)
                availableRooms.Add(room);

        return availableRooms;
    }
}