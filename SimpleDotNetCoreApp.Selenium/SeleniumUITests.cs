using System;
using System.IO;
using System.Reflection;
using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;

namespace SimpleDotNetCoreApp.Selenium
{
    public class SeleniumUITests : IDisposable
    {
        private static IWebDriver _webDriver = null;
        private static string _webAppBaseURL;
        // A website kept running and not changed. Use to show working in VS, remove if prefer to show failure
        private static string _defaultWebAppBaseURL = "https://localhost:5001/";
        
        public SeleniumUITests()
        {
            // Chrome specifics
            _webDriver = new ChromeDriver(@"C:\tools\selenium");

            //Set page load timeout to 20 seconds (occasionally 5 secs is too tight after a deployment)
            _webDriver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(20);

            try
            {
                // Get the URL for the current environment (e.g. Dev, QA, Prod) as set in the release environment
                string releaseEnvironmentAppBaseURL = Environment.GetEnvironmentVariable("WebAppName");
                if (releaseEnvironmentAppBaseURL != null)
                {
                    _webAppBaseURL = "https://" + releaseEnvironmentAppBaseURL + ".azurewebsites.net";
                    Console.WriteLine("WebApp Base URL found: " + _webAppBaseURL);
                }
                else
                {
                    // The environment variable exists but has no value, so use a default
                    _webAppBaseURL = _defaultWebAppBaseURL;
                    Console.WriteLine("WebApp Base URL not set, using default: " + _defaultWebAppBaseURL);
                }
            }
            catch (Exception Ex)
            {
                // The environment variable probably doesn't exist (might be called from within VS)
                Console.WriteLine("Exception thrown accessing environment variable: " + Ex.Message);
                Console.WriteLine("Using default: " + _defaultWebAppBaseURL);
                _webAppBaseURL = _defaultWebAppBaseURL;
            }
        }

        public void Dispose()
        {
            if (_webDriver != null)
            {
                _webDriver.Quit();
            }
        }


        [Fact]
        public void IndexTitleChromeTest()
        {
            string expectedTitle = "Azure Pipelines";
            
            _webDriver.Url = _webAppBaseURL + "/Home/Index";
            var titleElement = _webDriver.FindElement(By.TagName("H1"));


            Assert.Equal(expectedTitle, titleElement.Text);
        }

        [Fact]
        public void HomePageFoundChromeTest()
        {
            _webDriver.Url = _webAppBaseURL;

            string actualPageTitle = _webDriver.Title;
            string expectedPageTitle = "Home Page - Azure Pipelines Demo App";

            Assert.Equal(expectedPageTitle, actualPageTitle);
        }

        [Fact]
        public void AboutPageFoundChromeTest()
        {
            _webDriver.Url = _webAppBaseURL + "/Home/About";

            string actualPageTitle = _webDriver.Title;
            string expectedPageTitle = "About - Azure Pipelines Demo App";

            Assert.Equal(expectedPageTitle, actualPageTitle);
        }

        [Fact]
        public void ContactPageFoundChromeTest()
        {
            _webDriver.Url = _webAppBaseURL + "/Home/Contact";

            string actualPageTitle = _webDriver.Title;
            string expectedPageTitle = "Contact - Azure Pipelines Demo App";

            Assert.Equal(expectedPageTitle, actualPageTitle);
        }

        [Fact]
        public void SupportEmailAddressChromeTest()
        {
            string supportEmailAddress = "Support@example.com";

            _webDriver.Url = _webAppBaseURL + "/Home/Contact";
            var supportEmailElement = _webDriver.FindElement(By.PartialLinkText(supportEmailAddress));

            Assert.Equal(supportEmailAddress, supportEmailElement.Text);
        }

        [Fact]
        public void MarketingEmailAddressChromeTest()
        {
            string marketingEmailAddress = "Marketing@example.com";

            _webDriver.Url = _webAppBaseURL + "/Home/Contact";
            var marketingEmailElement = _webDriver.FindElement(By.PartialLinkText(marketingEmailAddress));

            Assert.Equal(marketingEmailAddress, marketingEmailElement.Text);
        }
    }
}
