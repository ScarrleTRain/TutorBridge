using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutorBridge.Areas.Identity.Data;
using TutorBridge.Models;
using TutorBridge.ViewModels;

namespace TutorBridge.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var model = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                model.Add(new UserViewModel
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

            ViewBag.Roles = await _roleManager.Roles
               .Select(r => r.Name)
               .ToListAsync();

            return View(model);
        }
    }
}
