using Hospital.Core.TimeOffRequests.Models;

namespace Hospital.Core.TimeOffRequests.Services;

public interface IApprovalHandler
{
    public void Handle(DoctorTimeOffRequest request);
    public IApprovalHandler SetNext(IApprovalHandler handler);
}