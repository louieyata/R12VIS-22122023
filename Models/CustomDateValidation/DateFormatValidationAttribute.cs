using System;
using System.ComponentModel.DataAnnotations;

namespace R12VIS.Models.CustomDateValidation
{
    public class DateFormatValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime date;

            if (value is DateTime)
            {
                date = (DateTime)value;
            }
            else if (value is string)
            {
                if (!DateTime.TryParse((string)value, out date))
                {
                    return new ValidationResult("Invalid date format.");
                }
            }
            else
            {
                return new ValidationResult("Invalid date format.");
            }
            return ValidationResult.Success;
        }
    }
}