using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TutorBridge.Areas.Identity.Data;

// Add profile data for application users by adding properties to the User class
public class User : IdentityUser
{
    public int UserId { get; set; }
    public string NameFirst { get; set; }
    public string NameLast { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public DateOnly BirthDate { get; set; }
    public string? Blurb { get; set; }
    public bool IsTutor { get; set; }
    public bool IsAdmin { get; set; }
}

