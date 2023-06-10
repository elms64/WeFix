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
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateModel(
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

            var owner = await _userManager.GetUserAsync(User);
            if (owner == null)
            {
                return NotFound("Owner not found.");
            }

            if (User.IsInRole("SysAdmin") || User.IsInRole("Manager") || User.IsInRole("Reception") || User.IsInRole("Technician"))
            {
                var specifiedCustomer = await _userManager.FindByEmailAsync(Appointment.Email);
                if (specifiedCustomer == null)
                {
                    ModelState.AddModelError("Appointment.Email", "The specified customer email was not found in the system.");
                    return Page();
                }
                Appointment.OwnerID = specifiedCustomer.Id;
                Appointment.FirstName = specifiedCustomer.FirstName;
                Appointment.Surname = specifiedCustomer.Surname;
            }
            else
            {
                Appointment.OwnerID = owner.Id;
                Appointment.FirstName = owner.FirstName;
                Appointment.Surname = owner.Surname;
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(User, Appointment, AppointmentOperations.Create);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            Appointment.Status = AppointmentStatus.Submitted; // Set the status to Pending

            _context.Appointment.Add(Appointment);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
