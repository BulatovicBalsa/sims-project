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

        public List<Doctor> GetAllDoctors()
        {
            return _doctorRepository.GetAll();
        }

        public void AddExamination(Examination examination)
        {
            _examinationRepository.Add(examination, true);
        }

        public List<Examination> FindAvailableExaminations(Patient patient, ExaminationSearchOptions options)
        {
            List<Examination> examinations = SearchByBothCriteria(patient, options);
            
            if (examinations.Count>0) return examinations;
            if (options.Priority == Priority.Doctor)
            {
                examinations = SearchByDoctorPriority(patient, options);
                if(examinations.Count > 0) return examinations;
                examinations = SearchByTimeRangePriority(patient, options);
                if(examinations.Count > 0) return examinations;
            }
            else
            {
                examinations = SearchByTimeRangePriority(patient, options);
                if (examinations.Count > 0) return examinations;
                examinations = SearchByDoctorPriority(patient, options);
                if (examinations.Count > 0) return examinations;
            }
            return SearchWithoutPriority(patient, options);

        }

        private List<Examination> SearchByBothCriteria(Patient patient,ExaminationSearchOptions options)
        {
            List<Examination> examinations = new List<Examination>();
            DateTime currentDate = DateTime.Now.Date.AddDays(1);

            while(currentDate <= options.LatestDate)
            {
                DateTime startTime = currentDate.Add(options.StartTime);
                DateTime endTime = currentDate.Add(options.EndTime);
                DateTime currentTime = startTime;

                while(currentTime <= endTime)
                {
                    if(IsExaminationTimeFree(options.PreferredDoctor,patient,currentTime))
                    {
                        Examination examination = new Examination(options.PreferredDoctor, patient, false, currentTime, null);
                        examinations.Add(examination);
                        if (examinations.Count >= NUMBER_OF_SUGGESTED_EXAMINATIONS) return examinations;
                        
                    }
                    currentTime = currentTime.AddMinutes(1);
                }
                currentDate = currentDate.AddDays(1);
            }
            return examinations;
        }

        private List<Examination> SearchByDoctorPriority(Patient patient,ExaminationSearchOptions options)
        {
            List<Examination> examinations = new List<Examination>();
            DateTime currentDate = DateTime.Now.Date.AddDays(1);
            
            while(currentDate <= options.LatestDate)
            {
                DateTime currentTime = currentDate.Add(options.StartTime);

                while(currentTime.Date <= currentDate.AddDays(1))
                {
                    if (IsExaminationTimeFree(options.PreferredDoctor, patient, currentTime))
                    {
                        Examination examination = new Examination(options.PreferredDoctor, patient, false, currentTime, null);
                        examinations.Add(examination);
                        if (examinations.Count >= NUMBER_OF_SUGGESTED_EXAMINATIONS) return examinations;
                    }
                    currentTime = currentTime.AddMinutes(1);
                }
            currentDate= currentDate.AddDays(1);
            }
            return examinations;
        }

        private List<Examination> SearchByTimeRangePriority(Patient patient,ExaminationSearchOptions options)
        {
            List<Examination> examinations = new List<Examination>();
            List<Doctor> doctors = GetAllDoctors();

            DateTime currentDate = DateTime.Now.Date.AddDays(1);
            DateTime currentTime = currentDate + options.StartTime;

            foreach(var doctor in doctors)
            {
                while(currentDate <= options.LatestDate)
                {
                    while(currentTime.TimeOfDay < options.EndTime)
                    {
                        if (IsExaminationTimeFree(doctor, patient, currentTime))
                        {
                            Examination examination = new Examination(doctor, patient, false, currentTime, null);
                            examinations.Add(examination);
                            if (examinations.Count >= NUMBER_OF_SUGGESTED_EXAMINATIONS) return examinations;
                        }
                        currentTime = currentTime.AddMinutes(1);
                    }
                    currentDate = currentDate.AddDays(1);
                    currentTime = currentDate + options.StartTime;
                }
            }
            return examinations;

        }
        
        private List<Examination> SearchWithoutPriority(Patient patient,ExaminationSearchOptions options)
        {
            List<Examination> examinations = new List<Examination>();
            List<Doctor> doctors = GetAllDoctors();

            DateTime currentDate = DateTime.Now.Date.AddDays(1);

            while(currentDate <= options.LatestDate)
            {
                foreach(Doctor doctor in doctors)
                {
                    if (IsExaminationTimeFree(doctor, patient, currentDate))
                    {
                        Examination examination = new Examination(doctor, patient, false, currentDate, null);
                        examinations.Add(examination);
                        if (examinations.Count >= NUMBER_OF_SUGGESTED_EXAMINATIONS) return examinations;
                    }
                }
                currentDate = currentDate.AddMinutes(1);
            }
            return examinations;
        }

        private bool IsExaminationTimeFree(Doctor doctor, Patient patient, DateTime examinationTime)
        {
            return _examinationRepository.IsFree(doctor, examinationTime) && _examinationRepository.IsFree(patient, examinationTime);
        }
    }
}
