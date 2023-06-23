using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Exceptions
{
    public class BookNotLoanedException : Exception
    {
        public BookNotLoanedException(string message) : base(message) { }
    }
}
