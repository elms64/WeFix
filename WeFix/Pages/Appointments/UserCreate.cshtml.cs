using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WeFix.Areas.Identity.Data;
using WeFix.Authorization;
using WeFix.Data;
using WeFix.Models;

namespace WeFix.Pages.Appointments
{
    [Authorize(Roles = "User")]
    public class UserCreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserCreateModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _authorizationService = authorizationService;
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Appointment Appointment { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var owner = await _userManager.FindByEmailAsync(User.Identity.Name);
            if (owner == null)
            {
                return NotFound("Owner not found.");
            }

            Appointment.OwnerID = owner.Id;
            Appointment.Status = AppointmentStatus.Submitted;

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                    User, Appointment, AppointmentOperations.Create);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            _context.Appointment.Add(Appointment);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
