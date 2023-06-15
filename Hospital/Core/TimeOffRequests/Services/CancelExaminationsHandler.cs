using Hospital.Core.PatientHealthcare.Repositories;
using Hospital.Core.Scheduling;
using Hospital.Core.TimeOffRequests.Models;
using Hospital.Core.Workers.Models;
using Hospital.Core.Workers.Repositories;
using Hospital.Injectors;
using Hospital.Serialization;

namespace Hospital.Core.TimeOffRequests.Services;

public class CancelExaminationsHandler : AbstractApprovalHandler
{
    public override void Handle(DoctorTimeOffRequest request)
    {
        var doctor =
            new DoctorRepository(SerializerInjector.CreateInstance<ISerializer<Doctor>>()).GetById(request.DoctorId);
        ExaminationRepository.Instance.Delete(doctor, new TimeRange(request.Start, request.End));
        base.Handle(request);
    }
}