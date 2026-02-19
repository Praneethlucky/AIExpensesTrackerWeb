using ExpenseTracker.ConnectionsFactory;
using ExpenseTracker.Models;
using ExpenseTracker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace ExpenseTracker.Pages
{
    public class AIInsightsModel : PageModel
    {
        private readonly UserService _userRepository;
        private readonly AIService _aIService;
        public MonthlyInsightResponse monthlyInsightResponse { get; set; }
        public string AiResult = null;
        public AIInsightsModel(UserService userRepository, AIService aIService)
        {
            _userRepository = userRepository;
            _aIService = aIService;
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var prompt = "Test";
            var expectedSchema = "{ \"result\": string }";

            AiResult = await GenerateMonthlyInsightAsync();

            monthlyInsightResponse = JsonSerializer.Deserialize<MonthlyInsightResponse>(AiResult);

            return Page();
        }
        public async Task<string> GenerateMonthlyInsightAsync()
        {
            var prompt = """
User Salary: 68000
Total Monthly Bills: 10000
Category Breakdown:
Housing: 5000
Entertainment: 1000
Health: 2000
Savings: 15000
""";

            var expectedSchema = """
{
  "predictedTotal": number,
  "trend": string,
  "riskLevel": string,
  "drivers": array,
  "recommendations": array
}
""";

            return await _aIService.GenerateAsync(prompt, expectedSchema);
        }
    }
}
