using KeyAppTestAssignment.Models;

namespace KeyAppTestAssignment.Interfaces;

public interface IGooglePlayScraper
{
    Task<IEnumerable<string>> GetAppPackagesAsync(AppArguments arguments);
}