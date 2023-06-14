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
    [Authorize]
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

        // Property to store User's First Name and Last Name
        public Dictionary<string, string> UserNames { get; set; }

        public async Task OnGetAsync()
        {
            var appointments = from c in Context.Appointment
                               select c;

            var isAuthorized = User.IsInRole(Constants.AppointmentManagersRole) ||
                                User.IsInRole(Constants.ReceptionRole) ||
                                User.IsInRole(Constants.TechnicianRole) ||
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

            // Fetch First Name and Last Name for each User
            UserNames = new Dictionary<string, string>();
            foreach (var appointment in Appointment)
            {
                var owner = await UserManager.FindByIdAsync(appointment.OwnerID);
                var firstName = owner?.FirstName;
                var surname = owner?.Surname;
                UserNames[appointment.OwnerID] = $"{firstName} {surname}";
            }
        }
    }


}
