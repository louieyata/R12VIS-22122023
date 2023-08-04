using System;
using System.ComponentModel.DataAnnotations;

namespace R12VIS.Models.CustomDateValidation
{
    public class BrithDateValidationAttribute : ValidationAttribute
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

            var currentDate = DateTime.Now;
            var numberOfYears = currentDate.Year - date.Year;
            var minDate = currentDate.AddYears(-150);
            var maxDate = currentDate;

            if (date > maxDate)
            {
                return new ValidationResult("Oops! It looks like the selected date is in the future."); //Date cannot be in the future
            }

            if (date < minDate)
            {
                return new ValidationResult("Oops! It looks like the selected date is too far."); // Date cant be more than 150 years old
            }
            if (numberOfYears < 5)
            {
                return new ValidationResult("Oops! It looks like age is less than 5 years old."); // Age cant be less than 5 years old
            }
            return ValidationResult.Success;
        }
    }
}