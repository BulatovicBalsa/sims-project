using System.Collections.Generic;
using System.Windows.Documents;
using Hospital.Models.Doctor;
using Hospital.Models.Requests;
using Hospital.Repositories.Requests;

namespace Hospital.Services.Requests
{
    public class DoctorTimeOffRequestService
    {
        private readonly DoctorTimeOffRequestRepository _requestRepository = DoctorTimeOffRequestRepository.Instance;

        public DoctorTimeOffRequestService() { }

        public List<DoctorTimeOffRequest> GetAll()
        {
            return _requestRepository.GetAll();
        }

        public List<DoctorTimeOffRequest> GetNonExpiredDoctorTimeOffRequests(Doctor doctor)
        {
            return _requestRepository.GetNonExpiredDoctorTimeOffRequests(doctor);
        }
    }
}
