using AventStack.ExtentReports;
using OpenQA.Selenium;
//using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;
using System;
using BDDSpecFlowProject.Helpers;
using BDDSpecFlowProject.Reporting;

[Binding]
public class Hooks
{
    private readonly ScenarioContext _scenarioContext;
    private IWebDriver _driver;
    private WebDriverWait _wait;

    public Hooks(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [BeforeTestRun]
    public static void BeforeTestRun()
    {
        ExtentReportManager.InitializeReport();
    }

    [BeforeScenario]
    public void BeforeScenario()
    {
        _driver = new FirefoxDriver();
        //_driver = new ChromeDriver();
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0); // ❌ Disable implicit wait
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10)); // ✅ Use explicit wait instead
        _scenarioContext["WebDriver"] = _driver;

        // Start test reporting for each scenario
        string scenarioName = _scenarioContext.ScenarioInfo.Title;
        ExtentReportManager.CreateTest(scenarioName);
    }

    [AfterStep]
    public void AfterStep()
    {
        string stepType = _scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
        string stepText = _scenarioContext.StepContext.StepInfo.Text;

        if (_scenarioContext.TestError == null)
        {
            // ✅ Log each step so they are clickable
            ExtentReportManager.LogTestStep(Status.Pass, $"✅ <b>{stepType}:</b> {stepText}");
        }
        else
        {
            // ✅ Capture screenshot if step fails
            string scenarioName = _scenarioContext.ScenarioInfo.Title;
            string screenshotPath = ScreenshotHelper.CaptureScreenshot(_driver, scenarioName);

            // ✅ Identify the exact cause of failure
            string errorMessage = GetFailureReason(_scenarioContext.TestError);

            // ✅ Log the failed step with the correct error message
            ExtentReportManager.LogTestStep(Status.Fail, $"❌ <b>{stepType}:</b> {stepText} <br> <b>Error:</b> {errorMessage}");

            // ✅ Attach screenshot (passing file path)
            if (!string.IsNullOrEmpty(screenshotPath))
            {
                ExtentReportManager.AttachScreenshot(screenshotPath);
            }
        }
    }

    /// <summary>
    /// Returns the exact failure reason based on the caught exception.
    /// </summary>
    private string GetFailureReason(Exception error)
    {
        if (error is NoSuchElementException)
            return "❌ Element not found! Possible incorrect XPath or selector.";
        if (error is ElementNotInteractableException)
            return "⚠️ Element is present but not interactable (possibly hidden).";
        if (error is StaleElementReferenceException)
            return "🔄 The element became stale before interaction (DOM changed).";
        if (error is TimeoutException)
            return "⏳ The element took too long to appear or load.";

        return error.Message; // Default error message for unknown exceptions
    }

    [AfterScenario]
    public void AfterScenario()
    {
        _driver.Quit();
    }

    [AfterTestRun]
    public static void AfterTestRun()
    {
        ExtentReportManager.FinalizeReport();
    }
}
