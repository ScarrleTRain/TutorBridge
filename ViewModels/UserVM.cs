using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TutorBridge.Validation;

namespace TutorBridge.ViewModels
{
    public class UserVM
    {
        public required string Id { get; set; }
        public required string NameFirst { get; set; }
        public required string NameLast { get; set; }
        public required string Email { get; set; }
        public string? Phone { get; set; }
        public DateOnly BirthDate { get; set; }
        public string? Blurb { get; set; }
        public required string Role { get; set; }
    }
}
