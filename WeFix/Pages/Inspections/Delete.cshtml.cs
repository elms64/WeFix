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
    public class DeleteModel : PageModel
    {
        private readonly WeFix.Data.ApplicationDbContext _context;

        public DeleteModel(WeFix.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Inspection == null)
            {
                return NotFound();
            }
            var inspection = await _context.Inspection.FindAsync(id);

            if (inspection != null)
            {
                Inspection = inspection;
                _context.Inspection.Remove(Inspection);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
