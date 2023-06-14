using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WeFix.Data;

namespace WeFix.Pages.Inspections
{
    public class CreateModel : PageModel
    {
        private readonly WeFix.Data.ApplicationDbContext _context;

        public CreateModel(WeFix.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Inspection Inspection { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Inspection == null || Inspection == null)
            {
                return Page();
            }

            _context.Inspection.Add(Inspection);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
