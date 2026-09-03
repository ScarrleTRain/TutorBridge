using System.ComponentModel.DataAnnotations;
using TutorBridge.Validation;

namespace TutorBridge.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "First name is required")]
        [StringLength(30, ErrorMessage = "Max 30 Characters")]
        [RegularExpression(@"^[a-zA-Z\s-]+$", ErrorMessage = "First name can only contain letters")]
        [Display(Name = "First Name")]
        public string NameFirst { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(30, ErrorMessage = "Max 30 Characters")]
        [RegularExpression(@"^[a-zA-Z\s-]+$", ErrorMessage = "Last name can only contain letters")]
        [Display(Name = "Last Name")]
        public string NameLast { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Phone")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [RegularExpression(@"^02\d{7,9}$", ErrorMessage = "Enter a valid NZ mobile number")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Birth Date is required")]
        [MinAge(5)]
        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        public DateOnly BirthDate { get; set; }

        [StringLength(500, ErrorMessage = "Max 500 characters")]
        [Display(Name = "Blurb")]
        public string? Blurb { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; } = string.Empty;
    }
}