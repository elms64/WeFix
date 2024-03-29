﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WeFix.Areas.Identity.Data;
using WeFix.Data;
using WeFix.Models;

namespace WeFix.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class ManageVehiclesModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public List<Vehicle> UserVehicles { get; set; }

        [BindProperty]
        public VehicleInputModel VehicleInput { get; set; }

        public ManageVehiclesModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound("User not found.");
            }

            await LoadAsync(currentUser);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadAsync(await _userManager.GetUserAsync(User));
                return Page();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound("User not found.");
            }

            var existingVehicle = await _context.Vehicle.FirstOrDefaultAsync(v => v.VehicleReg == VehicleInput.VehicleReg);
            if (existingVehicle != null)
            {
                ModelState.AddModelError(string.Empty, "A vehicle with the same registration number already exists. Please enter a unique vehicle registration number or contact customer support.");
                await LoadAsync(currentUser);
                return Page();
            }

            var vehicle = new Vehicle
            {
                VehicleReg = VehicleInput.VehicleReg,
                CarModel = VehicleInput.CarModel,
                Year = VehicleInput.Year,
                Doors = VehicleInput.Doors,
                TransmissionType = VehicleInput.TransmissionType,
                EngineSize = VehicleInput.EngineSize,
                OwnerID = currentUser.Id
            };

            _context.Vehicle.Add(vehicle);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            UserVehicles = await _context.Vehicle.Where(v => v.OwnerID == user.Id).ToListAsync();
        }
    }

    public class VehicleInputModel
    {
        [Required]
        [Display(Name = "Vehicle Registration")]
        public string VehicleReg { get; set; }

        [Required]
        [Display(Name = "Suzuki Model")]
        public string CarModel { get; set; }

        [Required]
        public string Year { get; set; }

        [Required]
        public int Doors { get; set; }

        [Required]
        [Display(Name = "Transmission Type")]
        public string TransmissionType { get; set; }

        [Required]
        [Display(Name = "Engine Size")]
        public double EngineSize { get; set; }
    }
}
