using System.ComponentModel.DataAnnotations;
using WeFix.Areas.Identity.Data;

namespace WeFix.Models
{
    public class Vehicle
    {
        [Key]
        [Display(Name = "Vehicle Registration")]
        public string VehicleReg { get; set; }

        // user ID from AspNetUser table.
        public string OwnerID { get; set; }

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
