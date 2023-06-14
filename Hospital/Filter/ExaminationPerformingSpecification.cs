using System;
using Hospital.Models.Examination;

namespace Hospital.Filter;

public class ExaminationPerformingSpecification : ISpecification<Examination>
{
    private readonly DateTime _end;
    private readonly DateTime _start;

    public ExaminationPerformingSpecification(DateTime start, DateTime end)
    {
        _start = start;
        _end = end;
    }

    public bool IsSatisfied(Examination examination)
    {
        return (examination.Start >= _start && examination.End <= _end) || examination.IsPerformable();
    }
}