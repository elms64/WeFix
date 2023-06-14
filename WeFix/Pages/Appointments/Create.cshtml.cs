using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
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

            // Check if the specified customer email exists in the User table
            var specifiedCustomer = await _userManager.FindByEmailAsync(Appointment.Email);
            if (specifiedCustomer == null)
            {
                ModelState.AddModelError("Appointment.Email", "The specified customer email was not found in the system.");
                return Page();
            }

            // Retrieve the vehicle based on the provided VehicleReg
            var vehicle = await _context.Vehicle.FirstOrDefaultAsync(v => v.VehicleReg == Appointment.VehicleReg);
            if (vehicle == null || vehicle.OwnerID != specifiedCustomer.Id)
            {
                ModelState.AddModelError("Appointment.VehicleReg", "The specified vehicle does not belong to the customer.");
                return Page();
            }

            Appointment.OwnerID = specifiedCustomer.Id;
            Appointment.FirstName = specifiedCustomer.FirstName;
            Appointment.Surname = specifiedCustomer.Surname;
            Appointment.Status = AppointmentStatus.Submitted; // Set the status to Pending

            /*  var isAuthorized = await _authorizationService.AuthorizeAsync(User, Appointment, AppointmentOperations.Create);
              if (!isAuthorized.Succeeded)
              {
                  return Forbid();
              } */

            _context.Appointment.Add(Appointment);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }


    }
}
