using Hospital.Models.Doctor;
using Hospital.Models.Examination;
using Hospital.Models.Patient;
using Hospital.Repositories.Doctor;
using Hospital.Repositories.Examinaton;
using Hospital.Repositories.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Printing;
using System.Text;
using System.Threading.Tasks;


namespace Hospital.Services
{
    public class ExaminationRecommenderService
    {
        private const int NUMBER_OF_SUGGESTED_EXAMINATIONS = 5;

        private DoctorRepository _doctorRepository;
        private ExaminationRepository _examinationRepository;
     
        public ExaminationRecommenderService() 
        {
            _doctorRepository = new DoctorRepository();
            _examinationRepository = new ExaminationRepository();
        }
        public List<Examination> FindAvailableExaminations(Patient patient,ExaminationSearchOptions options)
        {
            var recommendedExaminations = new List<Examination>();
            var closestExaminations = new List<Examination>();
            var doctors = _doctorRepository.GetAll();

            IEnumerable<Doctor> searchOrder = GetSearchOrder(doctors,options);
            
            foreach(var doctor in searchOrder)
            {
                for(DateTime date = DateTime.Today; date<=options.LatestDate;date = date.AddDays(1))
                {                    
                    var doctorAllExaminations = _examinationRepository.GetAll(doctor)
                        .Where(exam => exam.Start.Date == date)
                        .OrderBy(exam => exam.Start)
                        .ToList();

                    TimeSpan lastEndTime = TimeSpan.Zero;

                    foreach(var currentExamination in doctorAllExaminations)
                    {
                        DateTime examinationTime = date.Add(lastEndTime);
                        if (_examinationRepository.IsFree(doctor, examinationTime) &&
                            _examinationRepository.IsFree(patient, examinationTime))
                        {
                            Examination examination = new Examination(doctor, patient, false, examinationTime);

                            if(IsPreferredExamination(doctor,examinationTime.TimeOfDay,options))
                            {
                                recommendedExaminations.Add(examination);
                                if (recommendedExaminations.Count >= NUMBER_OF_SUGGESTED_EXAMINATIONS) return recommendedExaminations;
                            }
                            else closestExaminations.Add(examination);
                        }
                        lastEndTime = currentExamination.End.TimeOfDay;
                    }
                }
            }

            return GetFinalExaminations(recommendedExaminations,closestExaminations);
        }

        private IEnumerable<Doctor> GetSearchOrder(IEnumerable<Doctor> doctors, ExaminationSearchOptions options)
        {
            if (options.Priority == Priority.Doctor)
            {
                return new[] { options.PreferredDoctor }.Concat(doctors.Where(doctor => !doctor.Equals(options.PreferredDoctor)));
            }
            else if (options.Priority == Priority.TimeRange)
            {
                return doctors;
            }
            else
            {
                return Enumerable.Empty<Doctor>();
            }
        }

        private bool IsPreferredExamination(Doctor doctor,TimeSpan time,ExaminationSearchOptions options)
        {
            return (options.Priority == Priority.Doctor && doctor.Equals(options.PreferredDoctor)) ||
           (options.Priority == Priority.TimeRange && time >= options.StartTime && time <= options.EndTime);
        }

        private List<Examination> GetFinalExaminations(List<Examination> recommendedExaminations,List<Examination> closestExaminations)
        {
            if(recommendedExaminations.Count == 0)
            {
                return closestExaminations
               .OrderBy(examination => examination.Start)
               .Take(NUMBER_OF_SUGGESTED_EXAMINATIONS)
               .ToList();
            }

            return recommendedExaminations;
        }

    }
}
