using Hospital.Models.Doctor;
using Hospital.Models.Examination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Provide the patient with another way of scheduling an appointment, where the patient enters the desired doctor, time range
//during the day during which it is convenient for him to come for the examination and the date by which the examination must take place at the latest. Next to
//of this information, the patient chooses the priority. Its priority can be doctor or time range. If he chooses
//doctor, the system will find an appointment with that doctor, which does not have to be in the desired time range.
//If the patient selects a time range, the system will find him a free appointment in the desired period, but he may
//will not be with the desired doctor. If the system does not find a free appointment according to priority, they are displayed to the patient
//three reviews that are closest to his wishes, where he chooses which one he wants to go to
//generate methods that will be used in the implementation of this functionality. The method that will be used to find all possible appointment
//should be implemented in the ExaminationRecommenderService class. The method should be called FindExamination and should return all possible\
//appointments. The method should take as parameters the desired doctor, the desired time range, the desired date and the priority. The method should
//return null if it does not find an appointment that meets the patient's wishes. The method should return list of all possible appointment if it finds it.



namespace Hospital.Services.Patient
{
    public class ExaminationRecommenderService
    {
        //generate methods that will be used in the implementation of this functionality. The method that will be used to find all possible appointment
        public ExaminationRecommenderService() { }
        public List<Examination> FindAvailableExaminations(Doctor doctor, DateTime startTime, DateTime endTime, DateTime date, string priority)
        {
            //generate logic that find all possible appointments


            return null;
        }

    }
}
