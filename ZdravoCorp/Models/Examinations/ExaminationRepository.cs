using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Models.Examinations
{
    class ExaminationRepository
    {
        public List<Examination> Examinations { get; set; }

        public ExaminationRepository()
        {
            Examinations = new List<Examination>();
        }

    }
}
