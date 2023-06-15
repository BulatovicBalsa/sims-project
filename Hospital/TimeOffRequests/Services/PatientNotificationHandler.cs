using System.Collections.Generic;
using Hospital.Injectors;
using Hospital.Notifications.Models;
using Hospital.Notifications.Repositories;
using Hospital.PatientHealthcare.Models;
using Hospital.PatientHealthcare.Repositories;
using Hospital.Scheduling;
using Hospital.Serialization;
using Hospital.TimeOffRequests.Models;
using Hospital.Workers.Models;
using Hospital.Workers.Repositories;

namespace Hospital.TimeOffRequests.Services;

public class PatientNotificationHandler : AbstractApprovalHandler
{
    public override void Handle(DoctorTimeOffRequest request)
    {
        var doctor =
            new DoctorRepository(SerializerInjector.CreateInstance<ISerializer<Doctor>>()).GetById(request.DoctorId);
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