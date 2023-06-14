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
    public class DetailsModel : PageModel
    {
        private readonly WeFix.Data.ApplicationDbContext _context;

        public DetailsModel(WeFix.Data.ApplicationDbContext context)
        {
            _context = context;
        }

      public Inspection Inspection { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Inspection == null)
            {
                return NotFound();
            }

            var inspection = await _context.Inspection.FirstOrDefaultAsync(m => m.Id == id);
            if (inspection == null)
            {
                return NotFound();
            }
            else 
            {
                Inspection = inspection;
            }
            return Page();
        }
    }
}
