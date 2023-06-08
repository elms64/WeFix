using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WeFix.Areas.Identity.Data;
using WeFix.Models;

namespace WeFix.Pages
{
    [Authorize(Roles = "SysAdmin")]
    public class RoleManagerModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public List<UserRoleViewModel> UserRoles { get; set; }

        public RoleManagerModel(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            UserRoles = new List<UserRoleViewModel>();
        }

        public async Task<IActionResult> OnGet()
        {
            var users = _userManager.Users.ToList();

            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                UserRoles.Add(new UserRoleViewModel
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    Surname = user.Surname,
                    Roles = userRoles.ToList()
                });
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAddRoles(string userId, string selectedRole, string returnUrl)
        {
            var user = await _userManager.FindByNameAsync(userId);

            if (user != null && !string.IsNullOrEmpty(selectedRole))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                if (!userRoles.Contains(selectedRole))
                {
                    await _userManager.AddToRoleAsync(user, selectedRole);
                }
            }

            return Redirect(returnUrl);
        }

        public async Task<IActionResult> OnPostRemoveRoles(string userId, string returnUrl)
        {
            var user = await _userManager.FindByNameAsync(userId);

            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                if (userRoles.Any())
                {
                    await _userManager.RemoveFromRolesAsync(user, userRoles);
                }
            }

            return Redirect(returnUrl);
        }

        public async Task<IActionResult> OnPostDeleteUserAsync(string userId, string returnUrl)
        {
            var user = await _userManager.FindByNameAsync(userId);

            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToPage(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error deleting user.");
                }
            }

            return RedirectToPage();
        }

    }
}
