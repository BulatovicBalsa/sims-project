using Hospital.Repositories.Doctor;
using Hospital.Repositories.Examination;
using Hospital.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Injectors;
using Hospital.Models.Doctor;
using Hospital.Models.Requests;
using Hospital.Serialization;

namespace Hospital.Services.Requests
{
    public class CancelExaminationsHandler: AbstractApprovalHandler
    {
        public override void Handle(DoctorTimeOffRequest request)
        {
            var doctor = new DoctorRepository(SerializerInjector.CreateInstance<ISerializer<Doctor>>()).GetById(request.DoctorId);
            ExaminationRepository.Instance.Delete(doctor, new TimeRange(request.Start, request.End));
            base.Handle(request);
        }
    }
}
