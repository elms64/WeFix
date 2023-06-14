using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WeFix.Areas.Identity.Data;
using WeFix.Authorization;
using WeFix.Data;
using WeFix.Models;

namespace WeFix.Pages.Appointments
{
    [Authorize]
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

            var owner = await _userManager.GetUserAsync(User);
            if (owner == null)
            {
                return NotFound("Owner not found.");
            }

            Appointment.OwnerID = owner.Id;
            Appointment.FirstName = owner.FirstName;
            Appointment.Surname = owner.Surname;
            Appointment.Email = owner.Email;
            Appointment.Status = AppointmentStatus.Submitted;

            // Validate VehicleReg and OwnerID
            var vehicle = await _context.Vehicle.FirstOrDefaultAsync(v =>
                v.VehicleReg == Appointment.VehicleReg && v.OwnerID == owner.Id);

            if (vehicle == null)
            {
                ModelState.AddModelError("Appointment.VehicleReg", "Invalid Vehicle Registration number. Ensure this vehicle is registered to you in account settings. Or contact customer support.");
                return Page();
            }

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
