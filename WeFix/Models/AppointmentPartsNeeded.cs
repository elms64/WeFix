using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeFix.Models
{
    public class AppointmentPartsNeeded
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Completed Appointment ID")]
        public int InspectionId { get; set; }

        [Display(Name = "Part ID")]
        public int PartId { get; set; }

        [Display(Name = "Quantity Used")]
        public int QuantityNeeded { get; set; }

        [Display(Name = "Total Cost")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TotalCost { get; set; }

        // Navigation properties
        //  public CompletedAppointment CompletedAppointment { get; set; }
        //  public Part Part { get; set; }
    }
}
