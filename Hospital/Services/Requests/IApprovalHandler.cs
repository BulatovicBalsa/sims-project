using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Models.Requests;

namespace Hospital.Services.Requests
{
    public interface IApprovalHandler
    {
        public void Handle(DoctorTimeOffRequest request);
        public IApprovalHandler SetNext(IApprovalHandler handler);
    }
}
