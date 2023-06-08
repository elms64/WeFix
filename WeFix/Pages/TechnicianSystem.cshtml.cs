using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WeFix.Data;
using Microsoft.EntityFrameworkCore;
using WeFix.Models;

namespace WeFix.Pages
{
    public class TechnicianSystemModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public TechnicianSystemModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<AppointmentSummary> AppointmentSummaries { get; set; }
        public int? SelectedAppointmentIndex { get; set; }

        public async Task OnGetAsync()
        {
            // Fetch the appointments and extract the summary information
            var appointments = await _context.Appointment.ToListAsync();
            AppointmentSummaries = appointments.Select(a => new AppointmentSummary
            {
                CustomerName = $"{a.FirstName} {a.Surname}",
                VehicleReg = a.VehicleReg,
                Date = a.Date,
                Description = a.Description
            }).ToList();

            // Get the selected appointment index from the query string parameter
            if (int.TryParse(Request.Query["selectedAppointment"], out int selectedAppointmentIndex))
            {
                SelectedAppointmentIndex = selectedAppointmentIndex;
            }
        }

        public class AppointmentSummary
        {
            public string CustomerName { get; set; }
            public string VehicleReg { get; set; }
            public DateTime Date { get; set; }
            public string Description { get; set; }
        }
    }
}
