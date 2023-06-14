using System.Collections.Generic;
using System.Windows.Documents;
using Hospital.Models;
using Hospital.Models.Doctor;
using Hospital.Models.Requests;
using Hospital.Repositories;
using Hospital.Repositories.Doctor;
using Hospital.Repositories.Examination;
using Hospital.Repositories.Requests;
using Hospital.Scheduling;

namespace Hospital.Services.Requests;

public class DoctorTimeOffRequestService
{
    private readonly DoctorTimeOffRequestRepository _requestRepository = DoctorTimeOffRequestRepository.Instance;

    private readonly IAcceptanceHandler _acceptanceHandler;

    public DoctorTimeOffRequestService()
    {
        var cancelExaminationsHandler = new CancelExaminationsHandler();
        var notifyPatientsHandler = new PatientNotificationHandler();
        _acceptanceHandler = notifyPatientsHandler;
        _acceptanceHandler.SetNext(cancelExaminationsHandler);
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

    public void Accept(DoctorTimeOffRequest request)
    {
        if (request.IsApproved)
            return;

        request.IsApproved = true;
        _requestRepository.Update(request);
        _acceptanceHandler.Handle(request);
    }

    public void Reject(DoctorTimeOffRequest request)
    {
        if (request.IsApproved)
            return;

        _requestRepository.Delete(request);
    }

    private void NotifyPatients(DoctorTimeOffRequest request)
    {
        var doctor = DoctorRepository.Instance.GetById(request.DoctorId); 
        var examinationsToBeCancelled = ExaminationRepository.Instance.GetExaminationsInTimeRange(doctor, new TimeRange(request.Start, request.End));
        var notificationRepository = new NotificationRepository();
        foreach (var examination in examinationsToBeCancelled)
        {
            notificationRepository.Add(new Notification(examination.Patient.Id,
                $"Examination by {examination.Doctor.FirstName} {examination.Doctor.LastName} at {examination.Start} has been cancelled."));
        }
    }
    private void CancelExaminationsInPeriod(DoctorTimeOffRequest request)
    {
        var doctor = DoctorRepository.Instance.GetById(request.DoctorId); 
        ExaminationRepository.Instance.Delete(doctor, new TimeRange(request.Start, request.End));
    }
}