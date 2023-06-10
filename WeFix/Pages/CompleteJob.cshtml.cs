using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WeFix.Data;
using WeFix.Models;

namespace WeFix.Pages
{
    public class CompleteJobModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CompleteJobModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public int AppointmentId { get; set; }

        public string CustomerName { get; set; }
        public string VehicleReg { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var appointment = await _context.Appointment.FindAsync(AppointmentId);
            if (appointment == null)
            {
                return NotFound();
            }

            CustomerName = $"{appointment.FirstName} {appointment.Surname}";
            VehicleReg = appointment.VehicleReg;
            Date = appointment.Date;
            Description = appointment.Description;

            return Page();
        }
    }
}
