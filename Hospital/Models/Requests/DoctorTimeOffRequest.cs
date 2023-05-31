using System;
using Hospital.Exceptions;

namespace Hospital.Models.Requests;

public class DoctorTimeOffRequest
{
    public DoctorTimeOffRequest()
    {
        Reason = "";
    }

    public DoctorTimeOffRequest(string reason, DateTime start, DateTime end, bool isApproved = false)
    {
        if (string.IsNullOrEmpty(reason))
            throw new UndefinedTimeOffReasonException("Reason can't be empty");
        Reason = reason.Trim();
        Start = start.Date;
        End = end.Date;
        IsApproved = isApproved;
        CheckValidity();
    }

    public string Reason { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public bool IsApproved { get; set; }

    private void CheckValidity()
    {
        if (string.IsNullOrEmpty(Reason))
            throw new UndefinedTimeOffReasonException("Reason must be defined");
        var difference = Start - DateTime.Today;
        if (difference.TotalDays < 2)
            throw new InvalidOperationException("The start date must be at least 2 days away from today.");
    }
}