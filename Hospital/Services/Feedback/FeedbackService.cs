using Hospital.Models.Feedback;
using Hospital.Repositories.Feedback;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Services.Feedback
{
    public class FeedbackService
    {
        private readonly HospitalFeedbackRepository _hospitalFeedbackRepository;
        private readonly DoctorFeedbackRepository _doctorFeedbackRepository;

        public FeedbackService()
        {
            _hospitalFeedbackRepository = HospitalFeedbackRepository.Instance;
            _doctorFeedbackRepository = DoctorFeedbackRepository.Instance;
        }

        public void SubmitHospitalFeedback(HospitalFeedback feedback)
        {
            // Validate the feedback data if needed

            // Add the feedback to the repository
            _hospitalFeedbackRepository.Add(feedback);
        }

        public void SubmitDoctorFeedback(DoctorFeedback feedback)
        {
            // Validate the feedback data if needed

            // Add the feedback to the repository
            _doctorFeedbackRepository.Add(feedback);
        }
    }
}
