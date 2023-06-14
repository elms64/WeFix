using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WeFix.Areas.Identity.Data;
using WeFix.Data;
using WeFix.Models;

namespace WeFix.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class ManageVehiclesModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        [BindProperty]
        public Vehicle Vehicle { get; set; }

        public List<Vehicle> UserVehicles { get; set; }

        public ManageVehiclesModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound("User not found.");
            }

            UserVehicles = await _context.Vehicle.Where(v => v.OwnerID == currentUser.Id).ToListAsync();

            if (UserVehicles == null)
            {
                UserVehicles = new List<Vehicle>(); // Initialize with an empty list
            }

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound("User not found.");
            }

            Vehicle.OwnerID = currentUser.Id;

            _context.Vehicle.Add(Vehicle);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}
