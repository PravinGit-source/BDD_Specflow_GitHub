using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

public class EndToEndFlow : BasePage
{
    private readonly WebDriverWait wait;

    public EndToEndFlow(IWebDriver driver) : base(driver)
    {
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }

    private IWebElement UsernameField => driver.FindElement(By.Id("username"));
    private IWebElement PasswordField => driver.FindElement(By.Name("password"));
    private IWebElement SignInButton => driver.FindElement(By.Id("signInBtn"));
    private IWebElement TermsCheckBox => driver.FindElement(By.Id("terms"));
    private IWebElement CheckoutLink => driver.FindElement(By.PartialLinkText("Checkout"));
    private IWebElement CountryField => driver.FindElement(By.Id("country"));
    private IWebElement PurchaseButton => driver.FindElement(By.XPath("//input[@value='Purchase']"));
    private IWebElement SuccessMessage => driver.FindElement(By.XPath("//strong[text()='Success!']"));

    public void NavigateToLoginPage()
    {
        string url = ConfigReader.GetApplicationUrl();
        driver.Navigate().GoToUrl(url);
        driver.Manage().Window.Maximize();
    }

    public void EnterCredentials()
    {
        string username = ExcelReader.GetCellValue(0, "Username");
        string password = ExcelReader.GetCellValue(0, "Password");
        EnterText(By.Id("username"), username);
        EnterText(By.Name("password"), password);
    }

    public void ClickSignInButton()
    {
        ClickElement(By.Id("terms"));
        ClickElement(By.Id("signInBtn"));
        wait.Until(ExpectedConditions.ElementIsVisible(By.PartialLinkText("Checkout")));
    }

    public List<string> AddProductsToCart(Table table)
    {
        List<string> selectedProducts = new List<string>();
        IList<IWebElement> products = driver.FindElements(By.TagName("app-card"));

        foreach (IWebElement product in products)
        {
            string productName = product.FindElement(By.CssSelector(".card-title a")).Text;
            if (table.Rows.Select(r => r["Product"]).Contains(productName))
            {
                product.FindElement(By.CssSelector(".card-footer button")).Click();
                selectedProducts.Add(productName);
            }
        }
        CheckoutLink.Click();
        return selectedProducts;
    }

    public void ProceedToCheckout()
    {
        driver.FindElement(By.XPath("//button[normalize-space()='Checkout']")).Click();
    }
    public List<string> GetCheckoutProducts()
    {
        wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.XPath("//h4/a")));
        IList<IWebElement> checkoutCards = driver.FindElements(By.XPath("//h4/a"));
        return checkoutCards.Select(p => p.Text).ToList();
    }

    public void SelectCountry(string country)
    {
        CountryField.SendKeys("ind");
        wait.Until(ExpectedConditions.ElementIsVisible(By.LinkText(country)));
        driver.FindElement(By.LinkText(country)).Click();
    }

    public void ConfirmPurchase()
    {
        IWebElement checkbox = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//label[@for='checkbox2']")));
        checkbox.Click();
        PurchaseButton.Click();
    }

    public bool VerifySuccessMessage()
    {
        return SuccessMessage.Text.Contains("Success");
    }
}
