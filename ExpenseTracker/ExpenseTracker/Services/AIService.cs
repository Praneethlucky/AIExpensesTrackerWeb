using ExpenseTracker.ConnectionsFactory;
using ExpenseTracker.Models;
using System.Runtime;
using System.Text;
using System.Text.Json;

namespace ExpenseTracker.Services
{
    public class AIService
    {
        private readonly AIConnectionFactory _aIConnectionFactory;
        private readonly AIModelSettings _aiModelSettings = new AIModelSettings();
        private readonly HttpClient _http;

        public AIService(AIConnectionFactory aIConnectionFactory, HttpClient http) 
        {
            _aIConnectionFactory = aIConnectionFactory;
            _aiModelSettings = _aIConnectionFactory.GetModelDetails();
            _http = http;
        }

        public void GetInsights()
        {
            
        }
        public async Task<string> GenerateAsync(
    string prompt,
    string expectedJsonSchema)
        {
            var fullPrompt = $"""
You are a financial AI assistant.

IMPORTANT RULES:
- Return STRICTLY valid JSON.
- Do NOT include markdown.
- Do NOT include explanations.
- Do NOT include backticks.
- Return ONLY raw JSON.
- If unsure, return an empty JSON object.

EXPECTED OUTPUT FORMAT:
{expectedJsonSchema}

USER DATA:
{prompt}
""";

            var body = new
            {
                contents = new[]
                {
            new
            {
                parts = new[]
                {
                    new { text = fullPrompt }
                }
            }
        },
                generationConfig = new
                {
                    maxOutputTokens = _aiModelSettings.MaxTokens,
                    temperature = 0.2, // 🔥 lower randomness for structured output
                    responseMimeType = "application/json" // 🔥 force JSON
                }
            };

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"https://generativelanguage.googleapis.com/v1beta/models/{_aiModelSettings.Model}:generateContent?key={_aiModelSettings.ApiKey}"
            )
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(body),
                    Encoding.UTF8,
                    "application/json")
            };

            var response = await _http.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var rawResponse = await response.Content.ReadAsStringAsync();

            var text = JsonDocument.Parse(rawResponse)
                .RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            return ExtractJson(text);
        }
        private string ExtractJson(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "{}";

            var firstBrace = input.IndexOf('{');
            var lastBrace = input.LastIndexOf('}');

            if (firstBrace >= 0 && lastBrace > firstBrace)
            {
                return input.Substring(firstBrace, lastBrace - firstBrace + 1);
            }

            return "{}";
        }

    }
}
