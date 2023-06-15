using System.Collections.Generic;
using Hospital.PatientHealthcare.Models;

namespace Hospital.Filter.Nurse;

public interface IPatientFilter
{
    List<Patient> Filter(List<Patient> patientsToFilter, object valueToCompare);
}