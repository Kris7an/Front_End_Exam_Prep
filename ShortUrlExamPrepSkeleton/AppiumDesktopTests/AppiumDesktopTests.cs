using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using NUnit.Framework;

namespace AppiumDesktopTests
{
    public class AppiumDesktopTests
    {
        private WindowsDriver<WindowsElement> driver;
        private AppiumOptions options;

        private const string appLocation = @"D:\ExamPrepResources\ShortURL-DesktopClient-v1.0.net6\ShortURL-DesktopClient.exe";
        private const string appiumServer = "http://127.0.0.1:4723/wd/hub";
        private const string appServerUrl = "https://shorturl.gabcho7.repl.co/api";


        [SetUp]
        public void PrepareApp()
        {
            options = new AppiumOptions();
            options.AddAdditionalCapability("app", appLocation);
            driver = new WindowsDriver<WindowsElement>(new Uri(appiumServer), options);  
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [TearDown]  
        public void CloseApp()
        {
            driver.Quit();
        }

        [Test]  
        public void Test_Create_New_Url()
        {
            var urlToAdd = "https://url" + DateTime.Now.Ticks + ".com";
            var inputAppUrl = driver.FindElementByAccessibilityId("textBoxApiUrl");
            inputAppUrl.Clear();
            inputAppUrl.SendKeys(appServerUrl);
            var connectButton = driver.FindElementByAccessibilityId("buttonConnect");
            connectButton.Click();

            var buttonAdd = driver.FindElementByAccessibilityId("buttonAdd");
            buttonAdd.Click();

            var textBooxUrl = driver.FindElementByAccessibilityId("textBoxURL");
            textBooxUrl.SendKeys(urlToAdd);

            var buttonCreate = driver.FindElementByAccessibilityId("buttonCreate");
            buttonCreate.Click();

            var resultField = driver.FindElementByName(urlToAdd);

            Assert.IsNotEmpty(resultField.Text);
            Assert.That(resultField.Text, Is.EqualTo(urlToAdd));
        }



    }
}