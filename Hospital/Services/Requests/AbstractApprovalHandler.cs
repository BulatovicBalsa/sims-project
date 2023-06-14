using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Hospital.Models.Requests;

namespace Hospital.Services.Requests
{
    public abstract class AbstractApprovalHandler: IApprovalHandler
    {
        private IApprovalHandler? _nextHandler;

        public IApprovalHandler SetNext(IApprovalHandler handler)
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
