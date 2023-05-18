using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeFix.Models;

public class Customer
{
    [Display(Name = "Customer ID")]
    //Declaring attributes for Appointments
    public int CustomerID { get; set; }

    [Display(Name = "First Name")]
    public string? FirstName { get; set; }

    [Display(Name = "Surname")]
    public string? Surname { get; set; }

    [Display(Name = "Email")]
    public string? Email { get; set; }

    //Validation rules appropriate for UK Landlines and Mobiles
    [Display(Name = "Phone Number")]
    public string? Phone { get; set; }

}