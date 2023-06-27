using System.ComponentModel.DataAnnotations;

namespace CollegeApp.Validators
{
    public class DateCheckAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var date = (DateTime?)value; // convert value to DateTime

            if (date < DateTime.Today)
            {
                return new ValidationResult("Date must be greater than or equal to today's date");
            }

            return ValidationResult.Success;
        }
    }
}
