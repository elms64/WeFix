using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeFix.Models;

public class Customer
{
    [Display(Name = "Customer ID")]
    public int CustomerID { get; set; }

    [Display(Name = "First Name")]
    public string? FirstName { get; set; }

    [Display(Name = "Surname")]
    public string? Surname { get; set; }

    [Display(Name = "Email")]
    public string? Email { get; set; }

    [Display(Name = "Phone Number")]
    public string? Phone { get; set; }

    [Display(Name = "Address Line 1")]
    public string? AddressLine1 { get; set; }

    [Display(Name = "Address Line 2")]
    public string? AddressLine2 { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    [Display(Name = "Postcode")]
    public string? Postcode { get; set; }


}