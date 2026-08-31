using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace TutorBridge.Validation
{
    public class AllowedFileAttribute : ValidationAttribute
    {
        private readonly long _maxSizeBytes;
        private readonly string[] _allowedContentTypes;

        public AllowedFileAttribute(long maxSizeBytes, params string[] allowedContentTypes)
        {
            _maxSizeBytes = maxSizeBytes;
            _allowedContentTypes = allowedContentTypes;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not IFormFile file)
            {
                return ValidationResult.Success; // no file uploaded — the field is optional
            }

            if (file.Length > _maxSizeBytes)
            {
                return new ValidationResult($"File must be smaller than {_maxSizeBytes / (1024 * 1024)}MB.");
            }

            if (!_allowedContentTypes.Contains(file.ContentType))
            {
                return new ValidationResult("Only JPEG and PNG images are allowed.");
            }

            return ValidationResult.Success;
        }
    }
}