﻿using Hospital.Models;
using System;

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
