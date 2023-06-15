using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using WeFix.Data;
using WeFix.Models;

namespace WeFix.Pages
{
    [Authorize]
    public class TechnicianSystemModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public TechnicianSystemModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<AppointmentSummary> AppointmentSummaries { get; set; }
        public IList<Part> Parts { get; set; } // Add property for parts

        public int? SelectedAppointmentIndex { get; set; }

        public async Task OnGetAsync(int? selectedAppointment)
        {
            var appointments = await _context.Appointment
                .Where(a => a.Status == AppointmentStatus.Approved)
                .ToListAsync();

            var currentDate = DateTime.Today;

            // Filter appointments for today and future
            var todayAppointments = appointments.Where(a => a.Date.Date == currentDate).ToList();
            var futureAppointments = appointments.Where(a => a.Date.Date > currentDate).ToList();

            // Map appointment summaries for today's appointments
            var todayAppointmentSummaries = todayAppointments.Select(a => new AppointmentSummary
            {
                AppointmentId = a.Id,
                CustomerName = $"{a.FirstName} {a.Surname}",
                VehicleReg = a.VehicleReg,
                Date = a.Date,
                Description = a.Description,
                Status = a.Status // Include appointment status in summary
            }).ToList();

            // Map appointment summaries for future appointments
            var futureAppointmentSummaries = futureAppointments.Select(a => new AppointmentSummary
            {
                AppointmentId = a.Id,
                CustomerName = $"{a.FirstName} {a.Surname}",
                VehicleReg = a.VehicleReg,
                Date = a.Date,
                Description = a.Description,
                Status = a.Status // Include appointment status in summary
            }).ToList();

            AppointmentSummaries = todayAppointmentSummaries.Concat(futureAppointmentSummaries).ToList();

            SelectedAppointmentIndex = selectedAppointment.HasValue
                ? FindSelectedAppointmentIndex(selectedAppointment.Value)
                : null;

            Parts = await _context.Part.ToListAsync(); // Fetch parts data from the database
        }

        private int? FindSelectedAppointmentIndex(int selectedAppointmentId)
        {
            for (int i = 0; i < AppointmentSummaries.Count; i++)
            {
                if (AppointmentSummaries[i].AppointmentId == selectedAppointmentId)
                {
                    return i;
                }
            }
            return null;
        }

        public IActionResult OnPostCompleteInspection(int appointmentId)
        {
            return RedirectToPage("/Inspections/Create", new { appointmentId });
        }

        public class AppointmentSummary
        {
            public int AppointmentId { get; set; }
            public string CustomerName { get; set; }
            public string VehicleReg { get; set; }
            public DateTime Date { get; set; }
            public string Description { get; set; }
            public AppointmentStatus Status { get; set; } // Include appointment status property
        }
    }
}
