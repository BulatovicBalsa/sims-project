using System;

namespace Hospital.Exceptions
{
    public class BookNotLoanedException : Exception
    {
        public BookNotLoanedException() : base()
        {
            
        }

        public BookNotLoanedException(string message) : base(message)
        {
            
        }
    }
}
