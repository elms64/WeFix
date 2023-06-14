using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WeFix.Data;
using WeFix.Models;
using WeFix.Authorization;
using WeFix.Areas.Identity.Data;

namespace WeFix.Pages.Appointments
{
    [Authorize]
    public class EditModel : DI_BasePageModel
    {
        public EditModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<ApplicationUser> userManager)
            : base(context, authorizationService, userManager)
        {
        }

        [BindProperty]
        public Appointment Appointment { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Appointment? appointment = await Context.Appointment.FirstOrDefaultAsync(
                                                             m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            Appointment = appointment;

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                      User, Appointment,
                                                      AppointmentOperations.Update);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Fetch Contact from DB to get OwnerID.
            var appointment = await Context
                .Appointment.AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (appointment == null)
            {
                return NotFound();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                     User, appointment,
                                                     AppointmentOperations.Update);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            Appointment.OwnerID = appointment.OwnerID;

            Context.Attach(Appointment).State = EntityState.Modified;

            if (Appointment.Status == AppointmentStatus.Approved)
            {
                // If the contact is updated after approval, 
                // and the user cannot approve,
                // set the status back to submitted so the update can be
                // checked and approved.
                var canApprove = await AuthorizationService.AuthorizeAsync(User,
                                        Appointment,
                                        AppointmentOperations.Approve);

                if (!canApprove.Succeeded)
                {
                    Appointment.Status = AppointmentStatus.Submitted;
                }
            }

            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
