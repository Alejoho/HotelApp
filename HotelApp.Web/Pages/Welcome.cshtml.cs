using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelApp.Web.Pages;

public class Welcome : PageModel
{
    [FromRoute] // Explicitly bind from route
    public string FirstName { get; set; }

    [FromRoute]
    public string LastName { get; set; }
    public void OnGet()
    {
        
    }
}