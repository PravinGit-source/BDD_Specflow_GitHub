using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System;
using System.IO;

namespace BDDSpecFlowProject.Reporting
{
    public static class ExtentReportManager
    {
        private static ExtentReports _extent;
        private static ExtentTest _test;
        private static string _reportPath;

        /// <summary>
        /// Initializes the Extent Report (Generates a new report file for each test execution).
        /// </summary>
        public static void InitializeReport()
        {
            // Create Reports directory if it doesn't exist
            string reportsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");
            if (!Directory.Exists(reportsDirectory))
            {
                Directory.CreateDirectory(reportsDirectory);
            }

            // ✅ Generate a new report file with a timestamp (to prevent overwriting old reports)
            _reportPath = Path.Combine(reportsDirectory, $"ExtentReport_{DateTime.Now:yyyyMMdd_HHmmss}.html");

            // Create and configure ExtentSparkReporter
            var sparkReporter = new ExtentSparkReporter(_reportPath);
            sparkReporter.Config.DocumentTitle = "Test Execution Report";
            sparkReporter.Config.ReportName = "Automation Test Report";

            // ✅ Ensure that steps are clickable (default behavior in ExtentReports 5+)
            sparkReporter.Config.Encoding = "UTF-8";

            // Create ExtentReports instance and attach reporter
            _extent = new ExtentReports();
            _extent.AttachReporter(sparkReporter);

            Console.WriteLine($"✅ Extent Report initialized at: {_reportPath}");
        }

        /// <summary>
        /// Creates a new test case entry in the report.
        /// </summary>
        public static ExtentTest CreateTest(string testName)
        {
            _test = _extent.CreateTest(testName);
            return _test;
        }

        /// <summary>
        /// Logs test steps in the report.
        /// </summary>
        public static void LogTestStep(Status status, string message)
        {
            _test?.Log(status, message);
        }

        /// <summary>
        /// Attach a screenshot immediately after a failed step.
        /// </summary>
        public static void AttachScreenshot(string screenshotPath, string stepDetails)
        {
            _test?.Log(Status.Fail, stepDetails);  // ✅ Log the failing step
            _test?.AddScreenCaptureFromPath(screenshotPath);  // ✅ Attach screenshot below the failed step
        }

        /// <summary>
        /// Finalizes and flushes the report.
        /// </summary>
        public static void FinalizeReport()
        {
            _extent.Flush();
            Console.WriteLine($"✅ Extent Report finalized at: {_reportPath}");

            // Automatically open the report after test execution
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                FileName = _reportPath,
                UseShellExecute = true
            });
        }

        public static void AttachScreenshot(string screenshotPath)
        {
            _test?.AddScreenCaptureFromPath(screenshotPath);
        }
    }
}
