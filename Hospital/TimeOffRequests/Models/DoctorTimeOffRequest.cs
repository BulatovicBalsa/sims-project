using System;
using Hospital.Exceptions;

namespace Hospital.TimeOffRequests.Models;

public class DoctorTimeOffRequest
{
    public DoctorTimeOffRequest()
    {
        Id = Guid.NewGuid().ToString();
        Reason = "";
        DoctorId = "";
    }

    public DoctorTimeOffRequest(string doctorId, string? reason, DateTime start, DateTime end, bool isApproved = false)
    {
        Id = Guid.NewGuid().ToString();
        Reason = reason?.Trim();
        Start = start.Date;
        End = end.Date;
        IsApproved = isApproved;
        DoctorId = doctorId;
        CheckValidity();
    }

    public string? Reason { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public bool IsApproved { get; set; }
    public string DoctorId { get; set; }
    public string Id { get; set; }

    private void CheckValidity()
    {
        if (string.IsNullOrEmpty(Reason))
            throw new UndefinedTimeOffReasonException("Reason must be defined");
        var difference = Start - DateTime.Today;
        if (difference.TotalDays < 2)
            throw new InvalidOperationException("The start date must be at least 2 days away from today.");
        if (Start > End)
            throw new InvalidOperationException("Start needs to be after End");
    }
}