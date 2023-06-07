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

        public async Task<IActionResult> OnPostRemoveRolesAsync(string userId)
        {
            var user = await _userManager.FindByNameAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var deleteList = roles.ToList();

            foreach (var roleName in deleteList)
            {
                var result = await _userManager.RemoveFromRoleAsync(user, roleName);
                if (!result.Succeeded)
                {
                    // Handle the error, e.g., log it or display an error message
                    return BadRequest();
                }
            }

            return RedirectToPage("RoleManager");
        }
    }
}
