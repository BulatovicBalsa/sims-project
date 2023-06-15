using Hospital.Injectors;
using Hospital.PatientHealthcare.Repositories;
using Hospital.Scheduling;
using Hospital.Serialization;
using Hospital.TimeOffRequests.Models;
using Hospital.Workers.Models;
using Hospital.Workers.Repositories;

namespace Hospital.TimeOffRequests.Services;

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