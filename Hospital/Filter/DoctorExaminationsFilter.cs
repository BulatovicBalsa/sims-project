using System.Collections.Generic;
using System.Linq;
using Hospital.Core.PatientHealthcare.Models;

namespace Hospital.Filter;

public class DoctorExaminationsFilter : IFilter<Examination>
{
    public List<Examination> Filter(List<Examination> examinations, ISpecification<Examination> specification)
    {
        return examinations.Where(specification.IsSatisfied).ToList();
    }
}