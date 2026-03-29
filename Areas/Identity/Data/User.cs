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
    [Required(ErrorMessage = "First name is required")]
    [StringLength(30, ErrorMessage = "Max 30 Characters")]
    public required string NameFirst { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(30, ErrorMessage = "Max 30 Characters")]
    public required string NameLast { get; set; }

    [Phone(ErrorMessage = "Invalid phone number")]
    public string? Phone { get; set; }
    //public string Email { get; set; } Inherited from IdentityUser

    [Required(ErrorMessage = "Birth Date is required")]
    public DateOnly BirthDate { get; set; }
    //public string Password { get; set; } UserManager.CreateAsync(user, password) use identity functions
    [StringLength(500, ErrorMessage = "Max 500 characters")]
    public string? Blurb { get; set; }
    public bool IsTutor { get; set; }
    public bool IsAdmin { get; set; }
}

