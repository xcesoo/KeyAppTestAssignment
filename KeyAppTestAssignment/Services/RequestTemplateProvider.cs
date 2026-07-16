using KeyAppTestAssignment.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace KeyAppTestAssignment.Services;

public class RequestTemplateProvider : IRequestTemplateProvider
{
    private readonly string _templatePath;
    private readonly ILogger<RequestTemplateProvider> _logger;

    public RequestTemplateProvider(IConfiguration configuration, ILogger<RequestTemplateProvider> logger)
    {
        _templatePath = configuration["TemplatePaths:SearchRequestPath"] ?? "Templates/SearchRequest.txt";
        _logger = logger;
    }
    public async Task<string> BuildAsync(string keyword, string androidId)
    {
        if (!File.Exists(_templatePath))
        {
            throw new FileNotFoundException("Search Request Template file not found", _templatePath);
        }
        
        var template = await File.ReadAllTextAsync(_templatePath);
        
        if (string.IsNullOrWhiteSpace(template))
        {
            throw new InvalidOperationException($"The template file at '{_templatePath}' is empty");
        }        
        return template
            .Replace("{KEYWORD}", keyword)
            .Replace("{ANDROID_ID}", androidId);
    }
}