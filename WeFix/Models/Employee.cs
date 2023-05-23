using System;
using System.ComponentModel.DataAnnotations;

namespace WeFix.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string? OwnerID { get; set; }
        public string? FirstName { get; set; }
        public string? Surname { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        public string? Role { get; set; }

    }
}