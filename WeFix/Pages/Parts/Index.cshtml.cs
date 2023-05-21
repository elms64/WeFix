using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WeFix.Data;
using WeFix.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;


namespace WeFix.Pages.Parts
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly WeFix.Data.ApplicationDbContext _context;

        public IndexModel(WeFix.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Part> Part { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; } = default!;

        public SelectList? CarModels { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? CarModelInput { get; set; }

        public async Task OnGetAsync()
        {
            // Use LINQ to get a list of car models.
            IQueryable<string> modelQuery = from m in _context.Part
                                            orderby m.CarModel
                                            select m.CarModel;

            //LINQ query used to select parts
            var parts = from m in _context.Part
                        select m;

            //Use the provided search term and run method on database 
            if (!string.IsNullOrEmpty(SearchString))
            {
                parts = parts.Where(s => s.PartName.Contains(SearchString));
            }

            if (!string.IsNullOrEmpty(CarModelInput))
            {
                parts = parts.Where(x => x.CarModel == CarModelInput);
            }
            CarModels = new SelectList(await modelQuery.Distinct().ToListAsync());
            Part = await parts.ToListAsync();

        }
    }
}
