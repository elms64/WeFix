using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WeFix.Data;

namespace WeFix.Pages.Inspections
{
    public class EditModel : PageModel
    {
        private readonly WeFix.Data.ApplicationDbContext _context;

        public EditModel(WeFix.Data.ApplicationDbContext context)
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

            var inspection =  await _context.Inspection.FirstOrDefaultAsync(m => m.Id == id);
            if (inspection == null)
            {
                return NotFound();
            }
            Inspection = inspection;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Inspection).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InspectionExists(Inspection.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool InspectionExists(int id)
        {
          return (_context.Inspection?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
