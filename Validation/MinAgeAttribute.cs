using System.ComponentModel.DataAnnotations;

namespace TutorBridge.Validation
{
    public class MinAgeAttribute : ValidationAttribute
    {
        private readonly int _years;

        public MinAgeAttribute(int years)
        {
            _years = years;
            ErrorMessage = $"Must be at least {years} years old";
        }

        public MinAgeAttribute(int years, string errorMessage)
        {
            _years = years;
            ErrorMessage = errorMessage;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext vc)
        {
            if (value is DateOnly birthDate)
            {
                var minDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-_years));
                var maxDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-100));
                if (birthDate > minDate)
                {
                    var message = ErrorMessage ?? $"Must be at least {_years} years old";
                    return new ValidationResult(message, new[] { vc.MemberName ?? string.Empty });
                }
                else if (birthDate < maxDate)
                {
                    var message = ErrorMessage ?? $"Must be at less than 100 years old";
                    return new ValidationResult(message, new[] { vc.MemberName ?? string.Empty });
                }
            }

            return ValidationResult.Success;
        }

    }
}
