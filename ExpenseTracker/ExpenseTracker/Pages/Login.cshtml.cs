using ExpenseTracker.API.Services;
using ExpenseTracker.Web.HttpRequests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Pages
{
    public class LoginModel : PageModel
    {
        private readonly GetRequests _getRequests;

        public LoginModel(GetRequests getRequests)
        {
            _getRequests = getRequests;
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

        public void OnPost()
        {
            var result = _getRequests.GetAsync<bool>($"https://localhost:44373/user/login?email={Email}&password={Password}").Result;
            if (result)
            {
                HttpContext.Session.SetString("IsLoggedIn", "true");
                HttpContext.Session.SetInt32("UserId", 1);
                RedirectToPage(ReturnUrl ?? "/Bills");
            }
            else
            {
                ErrorMessage = "Invalid email or password.";
            }
        }
    }
}