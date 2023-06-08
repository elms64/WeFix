using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WeFix.Data;
using WeFix.Models;
using WeFix.Areas.Identity.Data;
using WeFix.Authorization;

namespace WeFix.Pages.Appointments
{
    [Authorize]
    public class DetailsModel : DI_BasePageModel
    {
        public DetailsModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<ApplicationUser> userManager)
            : base(context, authorizationService, userManager)
        {
        }

        public Appointment Appointment { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Appointment? _appointment = await Context.Appointment.FirstOrDefaultAsync(m => m.AppointmentID == id);

            if (_appointment == null)
            {
                return NotFound();
            }
            Appointment = _appointment;

            var isAuthorized = User.IsInRole(Constants.AppointmentManagersRole) ||
                               User.IsInRole(Constants.AppointmentAdministratorsRole);

            var currentUserId = UserManager.GetUserId(User);

            if (!isAuthorized
                && currentUserId != Appointment.OwnerID
                && Appointment.Status != AppointmentStatus.Approved)
            {
                return Forbid();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, AppointmentStatus status)
        {
            var appointment = await Context.Appointment.FirstOrDefaultAsync(
                                                      m => m.AppointmentID == id);

            if (appointment == null)
            {
                return NotFound();
            }

            var contactOperation = (status == AppointmentStatus.Approved)
                                                       ? AppointmentOperations.Approve
                                                       : AppointmentOperations.Reject;

            var isAuthorized = await AuthorizationService.AuthorizeAsync(User, appointment,
                                        contactOperation);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }
            appointment.Status = status;
            Context.Appointment.Update(appointment);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
