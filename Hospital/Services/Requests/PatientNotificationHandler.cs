using System.Collections.Generic;
using Hospital.Models;
using Hospital.Models.Examination;
using Hospital.Models.Requests;
using Hospital.Repositories;
using Hospital.Repositories.Doctor;
using Hospital.Repositories.Examination;
using Hospital.Scheduling;

namespace Hospital.Services.Requests;

public class PatientNotificationHandler : AbstractApprovalHandler
{
    public override void Handle(DoctorTimeOffRequest request)
    {
        var doctor = DoctorRepository.Instance.GetById(request.DoctorId);
        var examinationsToBeCancelled =
            ExaminationRepository.Instance.GetExaminationsInTimeRange(doctor,
                new TimeRange(request.Start, request.End));
        NotifyPatients(examinationsToBeCancelled);

        base.Handle(request);
    }

    private static void NotifyPatients(List<Examination> examinationsToBeCancelled)
    {
        var notificationRepository = new NotificationRepository();
        foreach (var examination in examinationsToBeCancelled)
            notificationRepository.Add(new Notification(examination.Patient.Id,
                $"Examination by {examination.Doctor.FirstName} {examination.Doctor.LastName} at {examination.Start} has been cancelled."));
    }
}