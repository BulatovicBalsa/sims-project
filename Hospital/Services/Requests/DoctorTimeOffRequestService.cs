using System.Collections.Generic;
using System.Windows.Documents;
using Hospital.Injectors;
using Hospital.Models;
using Hospital.Models.Doctor;
using Hospital.Models.Requests;
using Hospital.Repositories;
using Hospital.Repositories.Doctor;
using Hospital.Repositories.Examination;
using Hospital.Repositories.Requests;
using Hospital.Scheduling;
using Hospital.Serialization;

namespace Hospital.Services.Requests;

public class DoctorTimeOffRequestService
{
    private readonly DoctorTimeOffRequestRepository _requestRepository = new DoctorTimeOffRequestRepository(SerializerInjector.CreateInstance<ISerializer<DoctorTimeOffRequest>>());

    private readonly IApprovalHandler _approvalHandler;

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