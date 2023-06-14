using System.ComponentModel.DataAnnotations;
using WeFix.Areas.Identity.Data;
using WeFix.Models;

public class Invoice
{
    [Key]
    public int Id { get; set; }

    [Display(Name = "Appointment ID")]
    public int AppointmentId { get; set; }

    [Display(Name = "Technician ID")]
    public string UserId { get; set; }

    [Display(Name = "Job Details")]
    public string JobDetails { get; set; }

    [Display(Name = "Completion Date")]
    public DateTime CompletionDate { get; set; }

    [Display(Name = "Parts Used Reference")]
    public string PartsUsedId { get; set; }

    [Display(Name = "Hours Spent")]
    public decimal LabourHours { get; set; }

    [Display(Name = "Hourly Rate")]
    public decimal LabourRate { get; set; }

    [Display(Name = "Parts Cost (Excl. VAT)")]
    public decimal PartsCost { get; set; }

    [Display(Name = "Labour Cost (Excl. VAT)")]
    public decimal LabourCost { get; set; }

    [Display(Name = "Total Cost (Excl. VAT)")]
    public decimal TotalCost { get; set; }

    [Display(Name = "VAT (%)")]
    public decimal VAT { get; set; } = 20; // Assuming standard VAT rate of 20%

    [Display(Name = "VAT Amount")]
    public decimal VATAmount => (PartsCost + LabourCost) * (VAT / 100);

    [Display(Name = "Total Cost (Incl. VAT)")]
    public decimal TotalCostIncludingVAT => (PartsCost + LabourCost) + VATAmount;

    // Navigation properties
    public Appointment Appointment { get; set; }
    public ApplicationUser Technician { get; set; }

    public ICollection<AppointmentPartsUsed> AppointmentParts { get; set; }
}
