using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WeFix.Data;
using WeFix.Models;

namespace WeFix.Pages.Parts
{
    public class DeleteModel : PageModel
    {
        private readonly WeFix.Data.ApplicationDbContext _context;

        public DeleteModel(WeFix.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Part Part { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Part == null)
            {
                return NotFound();
            }

            var part = await _context.Part.FirstOrDefaultAsync(m => m.ID == id);

            if (part == null)
            {
                return NotFound();
            }
            else
            {
                Part = part;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Part == null)
            {
                return NotFound();
            }
            var part = await _context.Part.FindAsync(id);

            if (part != null)
            {
                Part = part;
                _context.Part.Remove(Part);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
