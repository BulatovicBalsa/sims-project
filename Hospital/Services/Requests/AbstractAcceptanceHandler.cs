using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Hospital.Models.Requests;

namespace Hospital.Services.Requests
{
    public abstract class AbstractAcceptanceHandler: IAcceptanceHandler
    {
        private IAcceptanceHandler? _nextHandler;

        public IAcceptanceHandler SetNext(IAcceptanceHandler handler)
        {
            _nextHandler = handler;
            return handler;
        }

        public virtual void Handle(DoctorTimeOffRequest request)
        {
            _nextHandler?.Handle(request);
        }
    }
}
