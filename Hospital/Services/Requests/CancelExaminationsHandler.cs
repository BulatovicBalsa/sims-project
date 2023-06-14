using Hospital.Repositories.Doctor;
using Hospital.Repositories.Examination;
using Hospital.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Models.Requests;

namespace Hospital.Services.Requests
{
    public class CancelExaminationsHandler: AbstractApprovalHandler
    {
        public override void Handle(DoctorTimeOffRequest request)
        {
            var doctor = DoctorRepository.Instance.GetById(request.DoctorId);
            ExaminationRepository.Instance.Delete(doctor, new TimeRange(request.Start, request.End));
            base.Handle(request);
        }
    }
}
