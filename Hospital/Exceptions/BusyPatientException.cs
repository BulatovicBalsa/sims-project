using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Exceptions
{
    public class BusyPatientException : Exception
    {
        public BusyPatientException(string message) : base(message) { }
    }
}
