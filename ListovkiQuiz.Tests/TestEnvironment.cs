using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace ListovkiQuiz.Tests;

/// <summary>
/// Provides the hosting environment required by UserStore during tests.
/// Uses a temporary content directory instead of the application's actual data folder.
/// </summary>
internal class TestEnvironment : IWebHostEnvironment
{
    public TestEnvironment(string contentRootPath)
    {
        // Keep test data separate from the real application files.
        ContentRootPath = contentRootPath;
    }

    public string ContentRootPath { get; set; }

   
    public string ApplicationName { get; set; } = "ListovkiQuiz.Tests";
    public string EnvironmentName { get; set; } = "Testing";
    public string WebRootPath { get; set; } = "";
    public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
    public IFileProvider WebRootFileProvider { get; set; } = new NullFileProvider();
}
