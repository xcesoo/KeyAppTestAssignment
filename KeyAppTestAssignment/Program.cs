using KeyAppTestAssignment.Interfaces;
using KeyAppTestAssignment.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHttpClient<IGooglePlayScraper, GooglePlayScraper>(client =>
{
    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
});

builder.Services.AddTransient<IApplicationRunner, ApplicationRunner>();

builder.Logging.SetMinimumLevel(LogLevel.Information);

using var app = builder.Build();

var runner = app.Services.GetRequiredService<IApplicationRunner>();

await runner.RunAsync(args);