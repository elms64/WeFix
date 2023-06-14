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
using WeFix.Authorization;
using WeFix.Areas.Identity.Data;

namespace WeFix.Pages.Appointments
{
    [Authorize]
    public class DeleteModel : DI_BasePageModel
    {
        public DeleteModel(
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
            Appointment? _appointment = await Context.Appointment.FirstOrDefaultAsync(
                                                 m => m.Id == id);

            if (_appointment == null)
            {
                return NotFound();
            }
            Appointment = _appointment;

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                     User, Appointment,
                                                     AppointmentOperations.Delete);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var appointment = await Context
                .Appointment.AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (appointment == null)
            {
                return NotFound();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                     User, appointment,
                                                     AppointmentOperations.Delete);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            Context.Appointment.Remove(appointment);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
