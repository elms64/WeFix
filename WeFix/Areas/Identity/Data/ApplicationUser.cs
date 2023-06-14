using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using WeFix.Models;

namespace WeFix.Areas.Identity.Data
{
    // This class is used to extend the Identity API user with further attributes
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string? FirstName { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string? Surname { get; set; }

        public int UsernameChangeLimit { get; set; } = 10;

        public byte[]? ProfilePicture { get; set; }



    }
}
