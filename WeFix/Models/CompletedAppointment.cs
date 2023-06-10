using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WeFix.Areas.Identity.Data;

namespace WeFix.Models
{
    public class CompletedAppointment
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Appointment ID")]
        public int AppointmentId { get; set; }

        [Display(Name = "Technician ID")]
        public string TechnicianId { get; set; }

        [Display(Name = "Technician")]
        public string TechnicianName { get; set; }

        [Display(Name = "Completion Date")]
        public DateTime CompletionDate { get; set; }

        [Display(Name = "Job Details")]
        public string JobDetails { get; set; }

        // Navigation properties
        public Appointment Appointment { get; set; }
        public ApplicationUser Technician { get; set; }

        public ICollection<AppointmentPartsUsed> AppointmentParts { get; set; }
    }
}
