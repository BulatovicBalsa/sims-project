using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Hospital.ValidationRules
{
    public class StringNotNullOrWhitespaceRule: ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                return string.IsNullOrWhiteSpace((string)value) ? new ValidationResult(false, "Field cannot be empty.") : ValidationResult.ValidResult;
            }
            catch (Exception e)
            {
                return new ValidationResult(false,
                    $"Illegal characters or {e.Message}");
            }
        }
    }
}
