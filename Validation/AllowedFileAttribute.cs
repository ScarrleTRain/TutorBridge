using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace TutorBridge.Validation
{
    public class AllowedFileAttribute : ValidationAttribute
    {
        private const int HeaderBytesToRead = 16;

        private readonly long _maxSizeBytes;
        private readonly string[] _allowedContentTypes;

        // Magic-byte signatures for all the supported image types
        private static readonly Dictionary<string, byte[][]> _fileSignatures =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["image/jpeg"] = new[] { new byte[] { 0xFF, 0xD8, 0xFF } },
                ["image/png"] = new[] { new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A } },
                ["image/gif"] = new[]
                {
                    new byte[] { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 }, // GIF87a
                    new byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 }  // GIF89a
                },
                ["image/bmp"] = new[] { new byte[] { 0x42, 0x4D } },
            };

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

            if (!_allowedContentTypes.Contains(file.ContentType, StringComparer.OrdinalIgnoreCase))
            {
                return new ValidationResult("Only JPEG and PNG images are allowed.");
            }

            if (!HasValidSignature(file))
            {
                return new ValidationResult("The file content doesn't match a valid image format.");
            }

            return ValidationResult.Success;
        }

        // Checks for known magic bytes
        private static bool HasValidSignature(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var header = new byte[HeaderBytesToRead];
            var bytesRead = stream.Read(header, 0, header.Length);
            stream.Position = 0; 

            // WEBP is a RIFF container: "RIFF" at offset 0, "WEBP" at offset 8.
            if (file.ContentType.Equals("image/webp", StringComparison.OrdinalIgnoreCase))
            {
                return bytesRead >= 12
                    && header.Take(4).SequenceEqual(new byte[] { 0x52, 0x49, 0x46, 0x46 })
                    && header.Skip(8).Take(4).SequenceEqual(new byte[] { 0x57, 0x45, 0x42, 0x50 });
            }

            if (!_fileSignatures.TryGetValue(file.ContentType, out var signatures))
            {
                return false;
            }

            return signatures.Any(sig => bytesRead >= sig.Length && header.Take(sig.Length).SequenceEqual(sig));
        }
    }
}