using Microsoft.EntityFrameworkCore;
using WeFix.Data;

namespace WeFix.Models;


public class UserRoleViewModel
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; }
}
