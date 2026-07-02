using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutorBridge.Areas.Identity.Data;
using TutorBridge.ViewModels;
using TutorBridge.Models;

namespace TutorBridge.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;

        public UsersController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var model = new List<UserVM>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                model.Add(new UserVM
                {
                    Id = user.Id,
                    NameFirst = user.NameFirst,
                    NameLast = user.NameLast,
                    Email = user.Email ?? "",
                    Phone = user.Phone,
                    BirthDate = user.BirthDate,
                    Blurb = user.Blurb,
                    Role = roles.FirstOrDefault() ?? "No Role"
                });
            }
            return View(model);
        }
    }
}
