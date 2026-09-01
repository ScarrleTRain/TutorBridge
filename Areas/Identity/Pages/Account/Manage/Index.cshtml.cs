// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TutorBridge.Areas.Identity.Data;
using TutorBridge.Services;
using TutorBridge.Validation;

namespace TutorBridge.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public IndexModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        
        public bool HasProfilePhoto { get; set; }

        public string UserId { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "First name is required")]
            [StringLength(30, ErrorMessage = "Max 30 Characters")]
            [RegularExpression(@"^[a-zA-Z\s-]+$", ErrorMessage = "First name can only contain letters")]
            [Display(Name = "First name")]
            public string NameFirst { get; set; }

            [Required(ErrorMessage = "Last name is required")]
            [StringLength(30, ErrorMessage = "Max 30 Characters")]
            [RegularExpression(@"^[a-zA-Z\s-]+$", ErrorMessage = "Last name can only contain letters")]
            [Display(Name = "Last name")]
            public string NameLast { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "Birth Date is required")]
            [MinAge(5)]
            [DataType(DataType.Date)]
            [Display(Name = "Date of birth")]
            public DateOnly BirthDate { get; set; }

            [StringLength(500, ErrorMessage = "Max 500 characters")]
            [Display(Name = "Bio")]
            public string Blurb { get; set; }

            [AllowedFile(20 * 1024 * 1024, "image/jpeg", "image/png")]
            [Display(Name = "Profile photo")]
            public IFormFile ProfilePhoto { get; set; }
        }

        private async Task LoadAsync(User user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;
            UserId = user.Id;
            HasProfilePhoto = user.ProfilePhoto != null;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                NameFirst = user.NameFirst,
                NameLast = user.NameLast,
                BirthDate = user.BirthDate,
                Blurb = user.Blurb
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            user.NameFirst = Input.NameFirst;
            user.NameLast = Input.NameLast;
            user.BirthDate = Input.BirthDate;
            user.Blurb = Input.Blurb;

            if (Input.ProfilePhoto != null)
            {
                using var photoStream = Input.ProfilePhoto.OpenReadStream();
                user.ProfilePhoto = await ImageProcessing.ResizeAndEncodeAsync(photoStream);
                user.ProfilePhotoContentType = "image/jpeg";
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                StatusMessage = "Unexpected error when trying to update your profile.";
                return RedirectToPage();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRemovePhotoAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            user.ProfilePhoto = null;
            user.ProfilePhotoContentType = null;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                StatusMessage = "Unexpected error when trying to remove your photo.";
                return RedirectToPage();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile photo has been removed";
            return RedirectToPage();
        }
    }
}
