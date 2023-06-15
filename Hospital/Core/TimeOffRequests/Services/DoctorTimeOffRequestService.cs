using System.Collections.Generic;
using Hospital.Core.TimeOffRequests.Models;
using Hospital.Core.TimeOffRequests.Repositories;
using Hospital.Core.Workers.Models;
using Hospital.Injectors;
using Hospital.Serialization;

namespace Hospital.Core.TimeOffRequests.Services;

public class DoctorTimeOffRequestService
{
    private readonly IApprovalHandler _approvalHandler;

    private readonly DoctorTimeOffRequestRepository _requestRepository =
        new(SerializerInjector.CreateInstance<ISerializer<DoctorTimeOffRequest>>());

    public DoctorTimeOffRequestService()
    {
        var cancelExaminationsHandler = new CancelExaminationsHandler();
        var notifyPatientsHandler = new PatientNotificationHandler();
        _approvalHandler = notifyPatientsHandler;
        _approvalHandler.SetNext(cancelExaminationsHandler);
    }

    public List<DoctorTimeOffRequest> GetAll()
    {
        return _requestRepository.GetAll();
    }

    public List<DoctorTimeOffRequest> GetNonExpiredDoctorTimeOffRequests(Doctor doctor)
    {
        return _requestRepository.GetNonExpiredDoctorTimeOffRequests(doctor);
    }

    public void Add(DoctorTimeOffRequest request)
    {
        _requestRepository.Add(request);
    }

    public void Approve(DoctorTimeOffRequest request)
    {
        if (request.IsApproved)
            return;

        request.IsApproved = true;
        _requestRepository.Update(request);
        _approvalHandler.Handle(request);
    }

    public void Reject(DoctorTimeOffRequest request)
    {
        if (request.IsApproved)
            return;

        _requestRepository.Delete(request);
    }
}