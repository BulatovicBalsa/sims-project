using System.Collections.Generic;
using System.Linq;
using Hospital.Core.PatientHealthcare.Models;

namespace Hospital.Filter.Nurse;

public class PatientNameFilter : IPatientFilter
{
    public List<Patient> Filter(List<Patient> patientsToFilter, object valueToCompare)
    {
        var shouldContain = valueToCompare as string;
        var matchingPatients = patientsToFilter.Where(patient =>
            (patient.FirstName + patient.LastName).ToLower().Contains(shouldContain!.ToLower())).ToList();

        return matchingPatients;
    }
}