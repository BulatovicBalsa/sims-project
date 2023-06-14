using System.Collections.Generic;
using Hospital.Injectors;
using Hospital.Models;
using Hospital.Models.Doctor;
using Hospital.Models.Examination;
using Hospital.Models.Requests;
using Hospital.Repositories;
using Hospital.Repositories.Doctor;
using Hospital.Repositories.Examination;
using Hospital.Scheduling;
using Hospital.Serialization;

namespace Hospital.Services.Requests;

public class PatientNotificationHandler : AbstractApprovalHandler
{
    public override void Handle(DoctorTimeOffRequest request)
    {
        var doctor = new DoctorRepository(SerializerInjector.CreateInstance<ISerializer<Doctor>>()).GetById(request.DoctorId);
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