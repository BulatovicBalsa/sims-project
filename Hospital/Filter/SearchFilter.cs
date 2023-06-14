using Hospital.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Filter
{
    public class SearchFilter
    {
        public static bool IsPersonMatchingFilter(Person person, string id, string searchText)
        {
            return person.Id != id &&
                   (person.FirstName.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    person.LastName.Contains(searchText, StringComparison.OrdinalIgnoreCase));
        }
    }
}
