using System;
using WeFix.Authorization;
using WeFix.Data;
using WeFix.Models;
using WeFix.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WeFix.Pages.Appointments
{

    public class IndexModel : DI_BasePageModel
    {
        public IndexModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<ApplicationUser> userManager)
            : base(context, authorizationService, userManager)
        {
        }

        public IList<Appointment> Appointment { get; set; }

        public async Task OnGetAsync()
        {
            var appointments = from c in Context.Appointment
                               select c;

            var isAuthorized = User.IsInRole(Constants.AppointmentManagersRole) ||
                               User.IsInRole(Constants.AppointmentAdministratorsRole);

            var currentUserId = UserManager.GetUserId(User);

            // Only approved contacts are shown UNLESS you're authorized to see them
            // or you are the owner.
            if (!isAuthorized)
            {
                appointments = appointments.Where(c => c.Status == AppointmentStatus.Approved
                                            || c.OwnerID == currentUserId);
            }

            Appointment = await appointments.ToListAsync();
        }
    }

}
