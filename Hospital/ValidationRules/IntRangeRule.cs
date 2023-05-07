using System;
using System.Globalization;
using System.Windows.Controls;

namespace Hospital.ValidationRules;

public class IntRangeRule : ValidationRule
{
    public int Min { get; set; }
    public int Max { get; set; }

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        var enteredValue = 0;

        try
        {
            if (((string)value).Length > 0)
                enteredValue = int.Parse((string)value);
        }
        catch (Exception e)
        {
            return new ValidationResult(false,
                $"Illegal characters or {e.Message}");
        }

        if (enteredValue < Min || enteredValue > Max)
            return new ValidationResult(false,
                $"Please enter a value in the range: {Min}-{Max}.");
        return ValidationResult.ValidResult;
    }
}