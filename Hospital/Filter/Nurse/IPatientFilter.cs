using System;
using System.Collections.Generic;
using Hospital.Models.Patient;

namespace Hospital.Filter.Librarian;
public interface IPatientFilter
{
    List<Patient> Filter(List<Patient> patientsToFilter, object valueToCompare);
}
