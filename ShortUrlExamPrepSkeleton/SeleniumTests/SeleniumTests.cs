using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V109.DOM;
using static System.Net.WebRequestMethods;
using NUnit.Framework;

namespace SeleniumTests
{
    public class SeleniumTests
    {
        private WebDriver driver;
        private const string baseUrl = "https://shorturl.gabcho7.repl.co/";

        [SetUp]
        public void OpenWebApp()
        {
            this.driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.Url = baseUrl;
        }

        [TearDown]
        public void CloseWebApp()
        {
            driver.Quit();
        }

        [Test]
        public void Test_TableTopLeftCell()
        {
            var linkShortUrl = driver.FindElement(By.LinkText("Short URLs"));
            linkShortUrl.Click();

            var tableHeaderLeftCell = driver.FindElement(By.XPath("//th[contains(.,'Original URL')]"));

            Assert.That(tableHeaderLeftCell.Text, Is.EqualTo("Original URL"));
        }

        [Test]
        public void Test_AddValidUrl()
        {
            var randomUrl = "https://url" + DateTime.Now.Ticks + ".com";

            var linkAddUrl = driver.FindElement(By.LinkText("Add URL"));
            linkAddUrl.Click();

            var tableHeaderLeftCell = driver.FindElement(By.XPath("//input[@id='url']"));
            tableHeaderLeftCell.SendKeys(randomUrl);

            var createButton = driver.FindElement(By.XPath("//button[@type='submit'][contains(.,'Create')]"));
            createButton.Click();

            var linkShortUrl = driver.FindElement(By.LinkText("Short URLs"));
            linkShortUrl.Click();

            //Simple solution
            Assert.That(driver.PageSource.Contains(randomUrl));

            //Harder solution if we have more than one element with same name
            //Copy -> Copy Selector
            var tableLastRow = driver.FindElements(By.CssSelector("table > tbody > tr")).Last();
            var tableLastRowFirstCell = tableLastRow.FindElements(By.CssSelector("td")).First();

            Assert.That(tableLastRowFirstCell.Text, Is.EqualTo(randomUrl));
        }

        [Test]
        public void Test_NavigateToWrongUrl()
        {
            driver.Url = "http://shorturl.nakov.repl.co/go/invalid53652";

            var errorPresent = driver.FindElement(By.XPath("//div[@class='err'][contains(.,'Cannot navigate to given short URL')]"));

            Assert.True(errorPresent.Displayed);
        }

        [Test]
        public void Test_UrlVisits()
        {
            var linkShortUrl = driver.FindElement(By.LinkText("Short URLs"));
            linkShortUrl.Click();

            //Copy -> Copy Selector
            var tableFirstRow = driver.FindElements(By.CssSelector("table > tbody > tr")).First();
            var currentVisits = int.Parse(tableFirstRow.FindElements(By.CssSelector("td")).Last().Text);

            var urlToVisit = driver.FindElement(By.XPath("//a[@class='shorturl'][contains(.,'http://shorturl.gabcho7.repl.co/go/nak')]"));
            urlToVisit.Click();

            driver.SwitchTo().Window(driver.WindowHandles[0]);

            driver.Navigate().Refresh();

            tableFirstRow = driver.FindElements(By.CssSelector("table > tbody > tr")).First();
            var updatedVisits = int.Parse(tableFirstRow.FindElements(By.CssSelector("td")).Last().Text);

            Assert.That(updatedVisits, Is.EqualTo(currentVisits + 1));
        }
    }
}
