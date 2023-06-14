using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WeFix.Data;

namespace WeFix.Pages.Inspections
{
    public class IndexModel : PageModel
    {
        private readonly WeFix.Data.ApplicationDbContext _context;

        public IndexModel(WeFix.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Inspection> Inspection { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Inspection != null)
            {
                Inspection = await _context.Inspection.ToListAsync();
            }
        }
    }
}
