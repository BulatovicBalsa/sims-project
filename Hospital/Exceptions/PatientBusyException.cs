using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Exceptions
{
    public class BookAlreadyLoanedException : Exception
    {
        public BookAlreadyLoanedException(string message) : base(message) { }
    }
}
