using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExpenseTracker.Pages
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            var isLoggedIn = HttpContext.Session.GetString("IsLoggedIn");

            if (isLoggedIn != "true")
            {
                return RedirectToPage("/Login");
            }

            return Page();
        }

    }
}
