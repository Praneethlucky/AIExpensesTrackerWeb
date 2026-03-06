using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using ExpenseTracker.Domain.Interfaces;
using ExpenseTracker.Infrastructure.Configuration;

namespace ExpenseTracker.Infrastructure.AI;

public class GeminiService : IAIProvider
{
    private readonly HttpClient _http;
    private readonly GeminiSettings _settings;

    public GeminiService(
        HttpClient http,
        IOptions<GeminiSettings> options)
    {
        _http = http;
        _settings = options.Value;
    }

    public async Task<T> GenerateAsync<T>(object input)
    {
        var response = await _http.PostAsJsonAsync(
            $"https://generativelanguage.googleapis.com/v1beta/models/{_settings.Model}:generateContent?key={_settings.ApiKey}",
            input);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<T>();
    }
}