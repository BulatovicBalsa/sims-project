using Hospital.PatientFeedback.Models;
using Hospital.PatientFeedback.Repositories;

namespace Hospital.PatientFeedback.Services;

public class FeedbackService
{
    private readonly DoctorFeedbackRepository _doctorFeedbackRepository;
    private readonly HospitalFeedbackRepository _hospitalFeedbackRepository;

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