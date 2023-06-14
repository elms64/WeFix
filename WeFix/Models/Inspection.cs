using System.ComponentModel.DataAnnotations;
using WeFix.Areas.Identity.Data;
using WeFix.Models;

public class Inspection
{
    [Key]
    public int Id { get; set; }

    [Display(Name = "Appointment ID")]
    public int AppointmentId { get; set; }

    [Display(Name = "Notes")]
    public string Notes { get; set; }

    [Display(Name = "Required Parts")]
    public string PartsNeededId { get; set; }

    [Display(Name = "Email")]
    public string Email { get; set; }



}
