using System.Collections.Generic;
using System.Windows.Documents;
using Hospital.Models.Doctor;
using Hospital.Models.Requests;
using Hospital.Repositories.Doctor;
using Hospital.Repositories.Examination;
using Hospital.Repositories.Requests;
using Hospital.Scheduling;

namespace Hospital.Services.Requests;

public class DoctorTimeOffRequestService
{
    private readonly DoctorTimeOffRequestRepository _requestRepository = DoctorTimeOffRequestRepository.Instance;

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
        CancelExaminationsInPeriod(request);
        _requestRepository.Update(request);
    }

    private void CancelExaminationsInPeriod(DoctorTimeOffRequest request)
    {
        var doctor = DoctorRepository.Instance.GetById(request.DoctorId); 
        ExaminationRepository.Instance.Delete(doctor, new TimeRange(request.Start, request.End));
    }
}