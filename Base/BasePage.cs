using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

public class BasePage
{
    protected readonly IWebDriver driver;
    protected readonly WebDriverWait wait;

    public BasePage(IWebDriver driver)
    {
        this.driver = driver;
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }

    // Open a specified URL
    public void OpenUrl(string url)
    {
        driver.Navigate().GoToUrl(url);
    }

    // Maximize the browser window
    public void MaximizeWindow()
    {
        driver.Manage().Window.Maximize();
    }

    // Explicit Wait for Element to be Clickable
    protected IWebElement WaitForElementToBeClickable(By locator)
    {
        return wait.Until(ExpectedConditions.ElementToBeClickable(locator));
    }

    // Explicit Wait for Element to be Visible
    protected IWebElement WaitForElementToBeVisible(By locator)
    {
        return wait.Until(ExpectedConditions.ElementIsVisible(locator));
    }

    // Click an element with wait
    protected void ClickElement(By locator)
    {
        WaitForElementToBeClickable(locator).Click();
    }

    // Enter text into a field with wait
    protected void EnterText(By locator, string text)
    {
        var element = WaitForElementToBeVisible(locator);
        element.Clear();
        element.SendKeys(text);
    }

    // Get element text with wait
    protected string GetElementText(By locator)
    {
        return WaitForElementToBeVisible(locator).Text;
    }
}