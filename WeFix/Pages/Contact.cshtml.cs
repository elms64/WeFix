using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
namespace WeFix.Pages;

[AllowAnonymous]
public class ContactModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public ContactModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {

    }
}

