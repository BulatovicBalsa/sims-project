using System;

namespace Library.Exceptions
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
