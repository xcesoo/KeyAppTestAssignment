using KeyAppTestAssignment.Interfaces;
using KeyAppTestAssignment.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace KeyAppTestAssignment.Services;

public class GooglePlayScraper : IGooglePlayScraper
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<GooglePlayScraper> _logger;
    private readonly string _baseUrl;
    
    public GooglePlayScraper(HttpClient httpClient, ILogger<GooglePlayScraper> logger, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        _baseUrl = configuration["ScraperSettings:BaseUrl"] ?? "https://play.google.com";
    }
    
    public Task<IEnumerable<string>> GetAppPackagesAsync(AppArguments arguments)
    {
        throw new NotImplementedException();
    }
}