﻿using System.Collections.Generic;
using Hospital.Core.PatientHealthcare.Models;
using Hospital.Core.PatientHealthcare.Repositories;
using Hospital.Core.TimeOffRequests.Models;
using Hospital.Core.Workers.Models;
using Hospital.Injectors;
using Hospital.Notifications.Models;
using Hospital.Notifications.Repositories;
using Hospital.Scheduling;
using Hospital.Serialization;
using Hospital.Workers.Repositories;

namespace Hospital.Core.TimeOffRequests.Services;

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