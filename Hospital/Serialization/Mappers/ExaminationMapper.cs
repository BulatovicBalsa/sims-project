using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Hospital.Models.Doctor;
using Hospital.Models.Examination;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Serialization.Mappers
{
    public class ExaminationMapper: ClassMap<Examination>
    {
        public class MyListConverter : ITypeConverter
        {
            public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
            {
                // Split the string into a list of strings
                var list = text.Split(';').ToList();

                return list;
            }

            public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
            {
                // Convert the list of strings to a comma-separated string
                var list = (List<string>)value;
                var str = string.Join(";", list);

                return str;
            }
        }
        public ExaminationMapper()
        {
            Map(e => e.Id).Name("Id");
            Map(e => e.IsOperation).Name("IsOperation");
            Map(e => e.Start).Name("Start");

            Map(e => e.Doctor.Id).Name("DoctorId");
            Map(e => e.Doctor.FirstName).Name("DoctorFirstName");
            Map(e => e.Doctor.LastName).Name("DoctorLastName");
            Map(e => e.Doctor.Jmbg).Name("DoctorJmbg");
            Map(e => e.Doctor.Profile.Password).Name("DoctorPassword");
            Map(e => e.Doctor.Profile.Username).Name("DoctorUsername");

            Map(e => e.Patient.Id).Name("PatientId");
            Map(e => e.Patient.FirstName).Name("PatientFirstName");
            Map(e => e.Patient.LastName).Name("PatientLastName");
            Map(e => e.Patient.Jmbg).Name("PatientJmbg");
            Map(e => e.Patient.Profile.Password).Name("PatientPassword");
            Map(e => e.Patient.Profile.Username).Name("PatientUsername");
            Map(e => e.Patient.IsBlocked).Name("PatientIsBlocked");
            Map(p => p.Patient.MedicalRecord.Weight).Name("PatientWeight");
            Map(p => p.Patient.MedicalRecord.Height).Name("PatientHeight");
            //Map(p => p.Patient.MedicalRecord.Allergies).Name("PatientAllergies").TypeConverter<MyListConverter>();
            //Map(p => p.Patient.MedicalRecord.MedicalHistory).Name("PatientMedicalHistory");
            //Map(p => p.Patient.MedicalRecord.Prescriptions).Name("PatientPrescriptions").TypeConverter<MyListConverter>(); ;
        }
    }
}
