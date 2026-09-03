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
using TutorBridge.Services;
using TutorBridge.ViewModels;

namespace TutorBridge.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly INotificationService _notificationService;

        public UsersController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, INotificationService notificationService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _notificationService = notificationService;
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

        [Authorize]
        public async Task<IActionResult> Photo(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user?.ProfilePhoto == null)
            {
                return NotFound();
            }

            return File(user.ProfilePhoto, user.ProfilePhotoContentType ?? "application/octet-stream");
        }

        // GET: Users/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = await ToUserViewModel(user);

            return View(model);
        }

        // GET: Users/Create
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Roles = await RoleDropdown();
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.Email,
                    Email = model.Email,
                    NameFirst = model.NameFirst,
                    NameLast = model.NameLast,
                    Phone = model.Phone,
                    BirthDate = model.BirthDate,
                    Blurb = model.Blurb,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.Role))
                    {
                        await _userManager.AddToRoleAsync(user, model.Role);
                    }

                    // This account was created directly by an admin rather than
                    // through self-registration, so prompt the user to change
                    // the password they were given.
                    await _notificationService.NotifyAccountCreatedByAdminAsync(user);

                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ViewBag.Roles = await RoleDropdown();
            return View(model);
        }

        // GET: Users/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                NameFirst = user.NameFirst,
                NameLast = user.NameLast,
                Email = user.Email ?? "",
                Phone = user.Phone,
                BirthDate = user.BirthDate,
                Blurb = user.Blurb,
                Role = roles.FirstOrDefault() ?? ""
            };

            ViewBag.Roles = await RoleDropdown();
            return View(model);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EditUserViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                user.NameFirst = model.NameFirst;
                user.NameLast = model.NameLast;
                user.Email = model.Email;
                user.UserName = model.Email;
                user.Phone = model.Phone;
                user.BirthDate = model.BirthDate;
                user.Blurb = model.Blurb;

                var updateResult = await _userManager.UpdateAsync(user);

                if (updateResult.Succeeded)
                {
                    var currentRoles = await _userManager.GetRolesAsync(user);
                    if (!currentRoles.Contains(model.Role))
                    {
                        await _userManager.RemoveFromRolesAsync(user, currentRoles);
                        await _userManager.AddToRoleAsync(user, model.Role);
                    }

                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ViewBag.Roles = await RoleDropdown();
            return View(model);
        }

        // GET: Users/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = await ToUserViewModel(user);

            return View(model);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                // Same pattern as BookingsController: this goes through the
                // context's soft-delete override, so it sets DeletedAt rather
                // than removing the row outright.
                await _userManager.DeleteAsync(user);
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<UserViewModel> ToUserViewModel(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            return new UserViewModel
            {
                Id = user.Id,
                NameFirst = user.NameFirst,
                NameLast = user.NameLast,
                Email = user.Email ?? "",
                Phone = user.Phone,
                BirthDate = user.BirthDate,
                Blurb = user.Blurb,
                Role = roles.FirstOrDefault() ?? "No Role"
            };
        }

        public async Task<IEnumerable<SelectListItem>> RoleDropdown()
        {
            return (await _roleManager.Roles
                .Select(r => new SelectListItem
                {
                    Value = r.Name,
                    Text = r.Name
                })
                .ToListAsync())
                .OrderBy(r => r.Text);
        }
    }
}