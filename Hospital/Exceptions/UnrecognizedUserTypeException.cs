using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Exceptions
{
    public class UnrecognizedUserTypeException : Exception
    {
        public UnrecognizedUserTypeException() { }

        public UnrecognizedUserTypeException(string message) : base(message) { }
    }
}
