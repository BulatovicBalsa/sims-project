using Hospital.TimeOffRequests.Models;

namespace Hospital.TimeOffRequests.Services;

public interface IApprovalHandler
{
    public void Handle(DoctorTimeOffRequest request);
    public IApprovalHandler SetNext(IApprovalHandler handler);
}