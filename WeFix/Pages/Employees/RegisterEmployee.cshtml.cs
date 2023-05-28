using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WeFix.Areas.Identity.Data;

namespace WeFix.Pages.Employees
{
    public class RegisterEmployeeModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterEmployeeModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public bool UserCreatedSuccessfully { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string? Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string? Password { get; set; }

            [Required]
            public string? FirstName { get; set; }

            [Required]
            public string? Surname { get; set; }

            [Required]
            public List<string>? Roles { get; set; }

            [Required]
            [DataType(DataType.Upload)]
            [Display(Name = "Profile Picture")]
            public Stream? ProfilePicture { get; set; }
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    FirstName = Input.FirstName,
                    Surname = Input.Surname
                };

                if (Input.ProfilePicture != null && Input.ProfilePicture.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await Input.ProfilePicture.CopyToAsync(memoryStream);
                        user.ProfilePicture = memoryStream.ToArray();
                    }
                }

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    // Add selected roles to the user
                    await _userManager.AddToRolesAsync(user, Input.Roles);

                    // Set the user creation status
                    UserCreatedSuccessfully = true;

                    // User created successfully
                    return RedirectToPage("/Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Set the user creation status
            UserCreatedSuccessfully = false;

            // If we reach this point, something went wrong
            return Page();
        }
    }
}
