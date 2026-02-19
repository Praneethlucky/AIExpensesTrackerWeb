using ExpenseTracker.ConnectionsFactory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

public class LoginModel : PageModel
{
    private readonly UserService _userService;

    public LoginModel(UserService userService)
    {
        _userService = userService;
    }

    [BindProperty]
    [Required, EmailAddress]
    public string Email { get; set; }

    [BindProperty]
    [Required]
    public string Password { get; set; }

    public string ErrorMessage { get; set; }

    [BindProperty(SupportsGet = true)]
    public bool Timeout { get; set; }

    [BindProperty(SupportsGet = true)]
    public string ReturnUrl { get; set; } = null;

    public void OnGet()
    {
        HttpContext.Session.Clear();
        
        if (Timeout)
        {
            ErrorMessage = "Your session expired due to idle timeout. Please log in again.";
        }
    }


    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userService.GetByEmailAsync(Email);

        if (user == null ||
            !PasswordHasher.Verify(Password, user.Value.PasswordHash))
        {
            ErrorMessage = "Invalid email or password";
            return Page();
        }
        HttpContext.Session.SetString("IsLoggedIn", "true");
        if (!string.IsNullOrEmpty(ReturnUrl))
        {
            return Redirect(ReturnUrl);
        }

        // TEMP: redirect after successful login
        return RedirectToPage("/Expenses");
    }
}
