using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;

namespace ExpenseTracker.Web.Pages
{
    public class BillsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BillsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<Bill> Bills { get; set; } = new();

        [BindProperty]
        public Bill NewBill { get; set; }

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var response = await client.GetAsync($"https://localhost:44373/api/bills/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Bills = JsonSerializer.Deserialize<List<Bill>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _httpClientFactory.CreateClient();

            var content = new StringContent(
                JsonSerializer.Serialize(NewBill),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync(
                "https://localhost:44373/api/bills",
                content);

            return RedirectToPage(); // reload list
        }
    }
}
