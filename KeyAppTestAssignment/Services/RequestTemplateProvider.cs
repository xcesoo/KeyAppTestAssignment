using KeyAppTestAssignment.Interfaces;

namespace KeyAppTestAssignment.Services;

public class RequestTemplateProvider : IRequestTemplateProvider
{
    public async Task<string> BuildAsync(string keyword, string androidId)
    {
        var template = await File.ReadAllTextAsync("Templates/SearchRequest.txt");
        return template
            .Replace("{KEYWORD}", keyword)
            .Replace("{ANDROID_ID}", androidId);
    }
}