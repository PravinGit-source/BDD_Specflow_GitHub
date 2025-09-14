using System;
using System.IO;
using OpenQA.Selenium;

namespace BDDSpecFlowProject.Helpers
{
    public static class ScreenshotHelper
    {
        public static string CaptureScreenshot(IWebDriver driver, string scenarioName)
        {
            try
            {
                string screenshotsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "Screenshots");
                if (!Directory.Exists(screenshotsDirectory))
                {
                    Directory.CreateDirectory(screenshotsDirectory);
                }

                // ✅ Ensure unique screenshot filenames to prevent overwriting
                string filePath = Path.Combine(screenshotsDirectory, $"{scenarioName}_{DateTime.Now:yyyyMMdd_HHmmss}.png");

                Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                File.WriteAllBytes(filePath, screenshot.AsByteArray);

                Console.WriteLine($"📸 Screenshot saved at: {filePath}");
                return filePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Error capturing screenshot: {ex.Message}");
                return string.Empty;
            }
        }
    }
}
