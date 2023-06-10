﻿using System;
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
        public int? SelectedAppointmentIndex { get; set; }

        public async Task OnGetAsync(int? selectedAppointment)
        {
            var appointments = await _context.Appointment.Where(a => a.Status == AppointmentStatus.Approved).ToListAsync();
            var currentDate = DateTime.Today;

            // Filter appointments for today and future
            var todayAppointments = appointments.Where(a => a.Date.Date == currentDate).ToList();
            var futureAppointments = appointments.Where(a => a.Date.Date > currentDate).ToList();

            // Map appointment summaries for today's appointments
            var todayAppointmentSummaries = todayAppointments.Select(a => new AppointmentSummary
            {
                AppointmentId = a.AppointmentID,
                CustomerName = $"{a.FirstName} {a.Surname}",
                VehicleReg = a.VehicleReg,
                Date = a.Date,
                Description = a.Description
            }).ToList();

            // Map appointment summaries for future appointments
            var futureAppointmentSummaries = futureAppointments.Select(a => new AppointmentSummary
            {
                AppointmentId = a.AppointmentID,
                CustomerName = $"{a.FirstName} {a.Surname}",
                VehicleReg = a.VehicleReg,
                Date = a.Date,
                Description = a.Description
            }).ToList();

            AppointmentSummaries = todayAppointmentSummaries.Concat(futureAppointmentSummaries).ToList();

            if (selectedAppointment.HasValue)
            {
                SelectedAppointmentIndex = FindSelectedAppointmentIndex(selectedAppointment.Value);
            }
            else
            {
                SelectedAppointmentIndex = null;
            }
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



        public IActionResult OnPostCompleteJob(int appointmentId)
        {
            return RedirectToPage("CompleteJob", new { appointmentId });
        }

        public class AppointmentSummary
        {
            public int AppointmentId { get; set; }
            public string CustomerName { get; set; }
            public string VehicleReg { get; set; }
            public DateTime Date { get; set; }
            public string Description { get; set; }
        }
    }
}