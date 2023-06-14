using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeFix.Models;

public class Appointment
{
    //Declaring attributes for Appointments
    public int Id { get; set; }

    // user ID from AspNetUser table.
    public string? OwnerID { get; set; }

    [Display(Name = "First Name")]
    public string? FirstName { get; set; }

    public string? Surname { get; set; }

    public string? Email { get; set; }

    [Display(Name = "Vehicle Registration Number")]
    public string? VehicleReg { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Date")]
    public DateTime Date { get; set; }

    [StringLength(1000, MinimumLength = 3), Required]
    [Display(Name = "Describe your issue or service type")]
    public string? Description { get; set; }


    public AppointmentStatus Status { get; set; }
}

public enum AppointmentStatus
{
    Submitted,
    Approved,
    Rejected,
    Inspected,
    OnHold,
    Completed
}


