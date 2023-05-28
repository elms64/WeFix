using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WeFix.Areas.Identity.Data;

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

        public void OnGet()
        {
            UserRoles = _userManager.Users.Select(user => new UserRoleViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                Surname = user.Surname,
                Roles = _userManager.GetRolesAsync(user).Result.ToList()
            }).ToList();
        }

        public async Task<IActionResult> OnPostAssignRoles(string userId, string[] selectedRoles)
        {
            var user = await _userManager.FindByNameAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            await _userManager.AddToRolesAsync(user, selectedRoles);
            return RedirectToPage();
        }

        [HttpPost]
        public async Task<IActionResult> OnPostRemoveRoles(string userId)
        {
            var user = await _userManager.FindByNameAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles);

            // Assign the "User" role to the user
            await _userManager.AddToRoleAsync(user, "User");

            // Save the changes to the user
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Failed to update user roles.");
                return Page();
            }

            // Redirect to the page
            return RedirectToPage();
        }




        public class UserRoleViewModel
        {
            public string UserName { get; set; }
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string Surname { get; set; }
            public List<string> Roles { get; set; }
        }
    }
}
