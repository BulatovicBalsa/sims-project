using Hospital.Core.TimeOffRequests.Models;

namespace Hospital.Core.TimeOffRequests.Services;

public abstract class AbstractApprovalHandler : IApprovalHandler
{
    private IApprovalHandler? _nextHandler;

    public IApprovalHandler SetNext(IApprovalHandler handler)
    {
        _nextHandler = handler;
        return handler;
    }

    public virtual void Handle(DoctorTimeOffRequest request)
    {
        _nextHandler?.Handle(request);
    }
}