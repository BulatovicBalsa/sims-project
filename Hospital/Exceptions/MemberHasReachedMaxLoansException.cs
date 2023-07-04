using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Exceptions
{
    public class MemberHasReachedMaxLoansException : Exception
    {
        public MemberHasReachedMaxLoansException() : base()
        {

        }

        public MemberHasReachedMaxLoansException(string message) : base(message)
        {

        }
    }
}
