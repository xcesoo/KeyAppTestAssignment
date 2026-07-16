using KeyAppTestAssignment.Models;

namespace KeyAppTestAssignment.Interfaces;

public interface IRequestTemplateProvider
{
    Task<string> BuildAsync(string keyword, string androidId);
}