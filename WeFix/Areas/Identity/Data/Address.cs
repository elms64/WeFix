using System;
using System.ComponentModel.DataAnnotations;

namespace WeFix.Areas.Identity.Data
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Address Owner")]
        public string UserId { get; set; } // Foreign key to ApplicationUser

        [Required]
        [Display(Name = "Address Line 1")]
        public string AddressLine1 { get; set; }

        [Required]
        [Display(Name = "Address Line 2")]
        public string AddressLine2 { get; set; }

        [Required]
        public string City { get; set; }

        public string State { get; set; }

        [Required]
        public string PostCode { get; set; }

        [Required]
        public string Country { get; set; }

        // Navigation property for the user
        public ApplicationUser User { get; set; }
    }

}

