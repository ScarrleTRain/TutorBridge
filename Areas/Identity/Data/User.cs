using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TutorBridge.Areas.Identity.Data;

// Add profile data for application users by adding properties to the User class
public class User : IdentityUser
{
    // Id inherited

    // Makes sure there is a First name, and that it is less than 30 chars, and only letters, spaces or hyphens.
    [Required(ErrorMessage = "First name is required")]
    [StringLength(30, ErrorMessage = "Max 30 Characters")]
    [RegularExpression(@"^[a-zA-Z\s-]+$", ErrorMessage = "First name can only contain letters")]
    public required string NameFirst { get; set; }

    // Makes sure there is a Last name, and that it is less than 30 chars, and only letters, spaces or hyphens.
    [Required(ErrorMessage = "Last name is required")]
    [StringLength(30, ErrorMessage = "Max 30 Characters")]
    [RegularExpression(@"^[a-zA-Z\s-]+$", ErrorMessage = "First name can only contain letters")]
    public required string NameLast { get; set; }

    // Makes sure there is a phone number, and that it begins with 02, and then 7 to 9 digits, and is a valid phone number.
    [Phone(ErrorMessage = "Invalid phone number")]
    [RegularExpression(@"^02\d{7,9}$", ErrorMessage = "Enter a valid NZ mobile number")]
    public string? Phone { get; set; }

    // Makes sure there is a Birth Date, and that it is a date datatype.
    [Required(ErrorMessage = "Birth Date is required")]
    [DataType(DataType.Date)]
    public DateOnly BirthDate { get; set; }

    //public string Password { get; set; } UserManager.CreateAsync(user, password) use identity functions

    // Makes sure there is a Blurb, and that it is less than 500 chars.
    [StringLength(500, ErrorMessage = "Max 500 characters")]
    public string? Blurb { get; set; }

    public string? ProfilePhoto { get; set; }
}

