using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Models;
using Hospital.Models.Requests;
using Hospital.Repositories.Doctor;
using Hospital.Repositories.Examination;
using Hospital.Repositories;
using Hospital.Scheduling;

namespace Hospital.Services.Requests
{
    public class PatientNotificationHandler: AbstractAcceptanceHandler
    {
        public override void Handle(DoctorTimeOffRequest request)
        {
            var doctor = DoctorRepository.Instance.GetById(request.DoctorId);
            var examinationsToBeCancelled = ExaminationRepository.Instance.GetExaminationsInTimeRange(doctor, new TimeRange(request.Start, request.End));
            var notificationRepository = new NotificationRepository();
            foreach (var examination in examinationsToBeCancelled)
            {
                notificationRepository.Add(new Notification(examination.Patient.Id,
                    $"Examination by {examination.Doctor.FirstName} {examination.Doctor.LastName} at {examination.Start} has been cancelled."));
            }

            base.Handle(request);
        }
    }
}
