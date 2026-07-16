using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using KeyAppTestAssignment.Exceptions;
using KeyAppTestAssignment.Interfaces;
using KeyAppTestAssignment.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace KeyAppTestAssignment.Services;

public class GooglePlayScraper : IGooglePlayScraper
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<GooglePlayScraper> _logger;
    private readonly IRequestTemplateProvider _requestTemplateProvider;
    private readonly string _baseUrl;
    private readonly string _baseAndroidId;

    public GooglePlayScraper(HttpClient httpClient, ILogger<GooglePlayScraper> logger, IConfiguration configuration,
        IRequestTemplateProvider requestTemplateProvider)
    {
        _httpClient = httpClient;
        _logger = logger;
        _baseUrl = configuration["ScraperSettings:BaseUrl"] ?? "https://play.google.com";
        _baseAndroidId = configuration["ScraperSettings:BaseAndroidId"] ?? "108936018694992149580";
        _requestTemplateProvider = requestTemplateProvider;
    }

    public async Task<IEnumerable<string>> GetAppPackagesAsync(AppArguments arguments)
    {
        _logger.LogInformation("Building HTTP request");
        var url = $"{_baseUrl}/_/PlayStoreUi/data/batchexecute?hl={arguments.Country}";
        
        string freq = await _requestTemplateProvider.BuildAsync(arguments.Keyword, _baseAndroidId);
        
        var content = new FormUrlEncodedContent(
            [new KeyValuePair<string, string>("f.req", freq)]
            );
        
        HttpResponseMessage response;
        try
        { 
            response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            throw new ScraperException($"Network error while calling Google Play API: {ex.Message}", ex);
        }
        
        var responseString = await response.Content.ReadAsStringAsync();
        _logger.LogInformation("Successfully received response");
        
        return ParsePackagesFromResponse(responseString);
    }

    private IEnumerable<string> ParsePackagesFromResponse(string responseText)
    {
        var regex = new Regex(@"(?<=\\\"")[A-Za-z][A-Za-z0-9_]*(?:\.[A-Za-z0-9_]+)+(?=\\\"")", RegexOptions.Compiled);
        var matches = regex.Matches(responseText);
        var packages = new HashSet<string>();

        foreach (Match match in matches)
        {
            if (!match.Success)
                continue;

            packages.Add(match.Value);
        }
        
        if (!packages.Any())
        {
            throw new ScraperException("Could not find any package names in the response.");
        }
        return packages;
    }
}