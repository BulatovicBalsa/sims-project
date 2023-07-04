using System;
using System.Globalization;
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
