using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Models.Doctors;
using ZdravoCorp.Models.Patients;

namespace ZdravoCorp.Models
{
    public class Examination
    {
        const int DURATION = 15;
        public Doctor Doctor{ get; set; }
        public Patient Patient{ get; set; }
    }
}
