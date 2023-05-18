using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using WeFix.Data;
using WeFix.Models;

namespace WeFix.Pages.Appointments
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly WeFix.Data.ApplicationDbContext _context;

        public IndexModel(WeFix.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Appointment> Appointment { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Appointment != null)
            {
                Appointment = await _context.Appointment.ToListAsync();
            }
        }
    }
}
