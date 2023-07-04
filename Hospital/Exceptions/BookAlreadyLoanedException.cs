using System;

namespace Hospital.Exceptions
{
    public class BookAlreadyLoanedException : Exception
    {
        public BookAlreadyLoanedException() : base()
        {
            
        }

        public BookAlreadyLoanedException(string message) : base(message)
        {

        }
    }
}
