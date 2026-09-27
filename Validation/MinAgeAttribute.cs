using System.ComponentModel.DataAnnotations;

namespace TutorBridge.Validation
{
    public class MinAgeAttribute : ValidationAttribute
    {
        private const int MaxYears = 100;

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
                var maxDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-MaxYears));
                if (birthDate > minDate)
                {
                    var message = ErrorMessage ?? $"Must be at least {_years} years old";
                    return new ValidationResult(message, new[] { vc.MemberName ?? string.Empty });
                }
                else if (birthDate < maxDate)
                {
                    var message = $"Must be {MaxYears} years old or younger";
                    return new ValidationResult(message, new[] { vc.MemberName ?? string.Empty });
                }
            }

            return ValidationResult.Success;
        }

    }
}