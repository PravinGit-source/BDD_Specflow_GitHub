using Microsoft.Extensions.Configuration;
using System;
using System.IO;

public class ConfigReader
{
    private static IConfigurationRoot _configuration;

    static ConfigReader()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // Set path to the project root
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true); // Load JSON

        _configuration = builder.Build();
    }

    public static string GetApplicationUrl()
    {
        return _configuration["ApplicationSettings:BaseUrl"]; // Read the URL
    }
}