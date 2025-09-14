using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using TechTalk.SpecFlow;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BDDSpecFlowProject.StepDefinitions
{
    [Binding]
    public class EndToEndFlowSteps
    {
        private readonly IWebDriver driver;
        private readonly EndToEndFlow loginPage;
        private readonly WebDriverWait wait;
        private readonly List<string> selectedProducts = new List<string>();

        public EndToEndFlowSteps(ScenarioContext scenarioContext)
        {
            driver = (IWebDriver)scenarioContext["WebDriver"];
            loginPage = new EndToEndFlow(driver);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Given(@"I am on the login page")]
        public void GivenIAmOnTheLoginPage()
        {
            loginPage.NavigateToLoginPage();
        }

        [When(@"I enter username and password")]
        public void WhenIEnterUsernameAndPassword()
        {
            loginPage.EnterCredentials();
        }

        [When(@"I click the sign-in button")]
        public void WhenIClickTheSignInButton()
        {
            loginPage.ClickSignInButton();
        }

        [Then(@"I should see the product page")]
        public void ThenIShouldSeeTheProductPage()
        {
            Assert.IsTrue(driver.Url.Contains("angularpractice"), "Product page not loaded.");
        }

        [When(@"I add the following products to the cart:")]
        public void WhenIAddTheFollowingProductsToTheCart(Table table)
        {
            selectedProducts.AddRange(loginPage.AddProductsToCart(table));
        }


        [When(@"I proceed to checkout")]
        public void WhenIProceedToCheckout()
        {
            Thread.Sleep(5000);
            loginPage.ProceedToCheckout();
        }

        [Then(@"the selected products should be in the checkout list")]
        public void ThenTheSelectedProductsShouldBeInTheCheckoutList()
        {
            Assert.That(loginPage.GetCheckoutProducts(), Is.EqualTo(selectedProducts), "Mismatch in selected products.");
        }

        [When(@"I enter ind in the country field")]
        public void WhenIEnterIndInTheCountryField()
        {
            loginPage.SelectCountry("India");
        }

        [When(@"I confirm the purchase")]
        public void WhenIConfirmThePurchase()
        {
            loginPage.ConfirmPurchase();
        }

        [Then(@"I should see a success message")]
        public void ThenIShouldSeeASuccessMessage()
        {
            Assert.IsTrue(loginPage.VerifySuccessMessage(), "Success message not displayed.");
        }
    }
}
