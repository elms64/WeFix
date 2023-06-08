using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeFix.Models
{
    public class AppointmentPartsUsed
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("CompletedAppointment")]
        public int CompletedAppointmentId { get; set; }

        [ForeignKey("Part")]
        public int UsedPartId { get; set; }

        public int QuantityUsed { get; set; }

        public CompletedAppointment CompletedAppointment { get; set; }
        public Part Part { get; set; }
    }
}
