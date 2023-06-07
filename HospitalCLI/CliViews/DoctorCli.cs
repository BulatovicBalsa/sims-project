using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Models.Doctor;
using Hospital.Services;

namespace HospitalCLI.CliViews
{
    public class DoctorCli
    {
        private DoctorService _doctorService = new();
        private Doctor? _doctor;

        public DoctorCli()
        {
            _doctor = _doctorService.GetById(Thread.CurrentPrincipal!.Identity!.Name!.Split('|')[0]);
        }
    }
}
