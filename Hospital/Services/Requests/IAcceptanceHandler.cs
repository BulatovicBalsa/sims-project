using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Models.Requests;

namespace Hospital.Services.Requests
{
    public interface IAcceptanceHandler
    {
        public void Handle(DoctorTimeOffRequest request);
        public IAcceptanceHandler SetNext(IAcceptanceHandler handler);
    }
}
