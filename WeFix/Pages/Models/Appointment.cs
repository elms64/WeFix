using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeFix.Models;

public class Appointment
{
    //Declaring attributes for Appointments
    public int AppointmentID { get; set; }

    [Display(Name = "Customer ID")]
    public string? CustomerID { get; set; }

    [Display(Name = "Vehicle ID")]
    public string? VehicleID { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Date")]
    public DateTime Date { get; set; }

    [StringLength(500, MinimumLength = 3), Required]
    [Display(Name = "Description")]
    public string? Description { get; set; }


}