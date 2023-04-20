using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Exceptions
{
    public class PatientNotBusyException : Exception
    {
        public PatientNotBusyException(string message) : base(message) { }
    }
}
