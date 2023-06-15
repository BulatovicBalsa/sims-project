using System.Collections.Generic;
using System.Linq;
using Hospital.PatientHealthcare.Models;

namespace Hospital.Filter.Nurse;

public class PatientAccommodationRoomFilter : IPatientFilter
{
    public List<Patient> Filter(List<Patient> patientsToFilter, object valueToCompare)
    {
        var roomId = valueToCompare as string;
        var matchingPatients = patientsToFilter
            .Where(patient => patient.GetActiveHospitalTreatmentReferral()!.RoomId == roomId).ToList();

        return matchingPatients;
    }
}