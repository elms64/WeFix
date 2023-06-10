using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeFix.Models
{
    public class AppointmentPartsUsed
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Completed Appointment ID")]
        public int CompletedAppointmentId { get; set; }

        [Display(Name = "Part ID")]
        public int PartId { get; set; }

        [Display(Name = "Part")]
        public string PartName { get; set; }

        [Display(Name = "Quantity Used")]
        public int QuantityUsed { get; set; }

        // Navigation properties
        public CompletedAppointment CompletedAppointment { get; set; }
        public Part Part { get; set; }
    }
}
