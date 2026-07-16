using KeyAppTestAssignment.Exceptions;
using KeyAppTestAssignment.Interfaces;
using KeyAppTestAssignment.Models;
using Microsoft.Extensions.Logging;

namespace KeyAppTestAssignment.Services;

public class ApplicationRunner : IApplicationRunner
{
    private readonly IGooglePlayScraper _scraper;
    private readonly ILogger<ApplicationRunner> _logger;

    public ApplicationRunner(IGooglePlayScraper scraper, ILogger<ApplicationRunner> logger)
    {
        _scraper = scraper;
        _logger = logger;
    }


    public async Task RunAsync(string[] args)
    { 
        if (!CheckArgs(args)) return;
        var appArgs = new AppArguments(args[0], args[1]);
        
        _logger.LogInformation("Searching for packages by keyword: <{AppArgsKeyword}> in region: <{AppArgsCountry}>", appArgs.Keyword, appArgs.Country);

        try
        {
            var packages = (await _scraper.GetAppPackagesAsync(appArgs)).ToList();
            
            _logger.LogInformation("Found {Count} packages", packages.Count());

            foreach (var pkg in packages)
            {
                Console.WriteLine(pkg);
            }
        }
        catch (ScraperException ex)
        {
            _logger.LogError(ex, "Scraper exception: {Message}", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Critical error: {Message}", ex.Message);
        }
        
    }

    private bool CheckArgs(string[] args)
    {
        if (args.Length < 2)
        {
            _logger.LogError("Invalid arguments");
            _logger.LogInformation("Example: KeyAppTestAssignment <keyword> <country>");
            return false;
        }
        return true;
    }
}