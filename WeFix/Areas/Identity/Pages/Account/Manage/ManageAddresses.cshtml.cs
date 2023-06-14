using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WeFix.Areas.Identity.Data;
using WeFix.Data;
using WeFix.Models;

namespace WeFix.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class ManageAddressesModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public List<Address> UserAddresses { get; set; }

        [BindProperty]
        public AddressInputModel AddressInput { get; set; }

        public ManageAddressesModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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

            await LoadAsync(currentUser);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadAsync(await _userManager.GetUserAsync(User));
                return Page();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound("User not found.");
            }

            var existingAddress = await _context.Address.FirstOrDefaultAsync(a => a.AddressLine1 == AddressInput.AddressLine1 && a.AddressLine2 == AddressInput.AddressLine2 && a.City == AddressInput.City && a.State == AddressInput.State && a.PostCode == AddressInput.PostCode && a.Country == AddressInput.Country);
            if (existingAddress != null)
            {
                ModelState.AddModelError(string.Empty, "An address with the same details already exists. Please enter a unique address or contact customer support.");
                await LoadAsync(currentUser);
                return Page();
            }

            var address = new Address
            {
                UserId = currentUser.Id,
                AddressLine1 = AddressInput.AddressLine1,
                AddressLine2 = AddressInput.AddressLine2,
                City = AddressInput.City,
                State = AddressInput.State,
                PostCode = AddressInput.PostCode,
                Country = AddressInput.Country
            };

            _context.Address.Add(address);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            UserAddresses = await _context.Address.Where(a => a.UserId == user.Id).ToListAsync();
        }
    }

    public class AddressInputModel
    {
        [Required]
        [Display(Name = "Address Line 1")]
        public string AddressLine1 { get; set; }

        [Required]
        [Display(Name = "Address Line 2")]
        public string AddressLine2 { get; set; }

        [Required]
        public string City { get; set; }

        public string State { get; set; }

        [Required]
        public string PostCode { get; set; }

        [Required]
        public string Country { get; set; }
    }
}
