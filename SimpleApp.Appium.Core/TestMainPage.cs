using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace SimpleApp.Appium.Core
{
    public class TestMainPage<T, W>: AppiumTest<T, W>
        where T : AppiumDriver<W>
        where W : IWebElement
    {

        public TestMainPage() : base() { }
        public TestMainPage(string profile, string device) : base(profile, device) {
            this.profile = profile;
            this.device = device;
        }
        private string profile;
        private string device;

        protected override T GetDriver()
        {
            // Implemented by platform specific class
            throw new NotImplementedException();
        }

        protected override void InitAppiumOptions(AppiumOptions appiumOptions)
        {
            // Implemented by platform specific class
            throw new NotImplementedException();
        }

        [SetUp()]
        public void SetupTest()
        {
            //appiumDriver.CloseApp();
            //appiumDriver.LaunchApp();
        }

        [Test()]
        public void TestLogin()
        {
            appiumDriver.FindElement(MobileBy.AccessibilityId("Login")).Click();
            appiumDriver.FindElement(MobileBy.AccessibilityId("UserName")).SendKeys("user@email.com");
            appiumDriver.FindElement(MobileBy.AccessibilityId("Password")).SendKeys("password");

            appiumDriver.FindElement(MobileBy.AccessibilityId("LoginButton")).Click();
            var text = GetElementText("StatusLabel"); // Android is "text"

            Assert.IsNotNull(text);
            Assert.IsTrue(text.StartsWith("Logging in", StringComparison.CurrentCulture));  
        }

        [Test()]
        public void TestAddItem()
        {
            appiumDriver.FindElement(MobileBy.AccessibilityId("Browse")).Click();
            appiumDriver.FindElement(MobileBy.AccessibilityId("AddToolbarItem")).Click();
            var itemNameField = appiumDriver.FindElement(MobileBy.AccessibilityId("ItemNameEntry"));
            itemNameField.SendKeys("todo");

            var itemDesriptionField = appiumDriver.FindElement(MobileBy.AccessibilityId("ItemDescriptionEntry"));
            itemDesriptionField.SendKeys("todo description");

            appiumDriver.FindElement(MobileBy.AccessibilityId("SaveToolbarItem")).Click();
        }

        [Test()]
        public void TestAbout()
        {
            waitForMobileElement((MobileBy)MobileBy.AccessibilityId("About"));
            appiumDriver.FindElement(MobileBy.AccessibilityId("About")).Click(); // works for iOS

            waitForWebElement(MobileBy.XPath("//android.widget.Button[@text='LEARN MORE']"));
            appiumDriver.FindElement(MobileBy.XPath("//android.widget.Button[@text='LEARN MORE']")).Click();

            waitForWebElement(MobileBy.XPath("//android.widget.LinearLayout/android.widget.TextView[@text='Chrome']"));
            appiumDriver.FindElement(MobileBy.XPath("//android.widget.LinearLayout/android.widget.TextView[@text='Chrome']")).Click();

            waitForWebElement(MobileBy.Id("android:id/button_always"));
            appiumDriver.FindElement(MobileBy.Id("android:id/button_always")).Click();
            Thread.Sleep(3000);
            List<string> AllContexts = new List<string>();
            foreach (var context in (appiumDriver.Contexts))
            {
                AllContexts.Add(context);
            }
            Console.WriteLine("Context count: " + AllContexts.Count+" Contains Webview? "+AllContexts.Contains("WEBVIEW"));

            appiumDriver.Context = "WEBVIEW_chrome";
            waitForWebElement(By.CssSelector(".navbar-toggler"));
            appiumDriver.FindElement(By.CssSelector(".navbar-toggler")).Click();

            waitForWebElement(By.XPath("//*[@id='navbarSupportedContent']/ul/li[3]/a"));
            appiumDriver.FindElement(By.XPath("//*[@id='navbarSupportedContent']/ul/li[3]/a")).Click();

            waitForWebElement(By.CssSelector("#navbarSupportedContent"));
            //waitForWebElement(By.CssSelector("#xamarin-forms"));

            IWebElement scrollElement = appiumDriver.FindElement(By.CssSelector("#navbarSupportedContent"));
            scrollToElementInWebView(scrollElement);
        }

        public void waitForMobileElement(MobileBy mobileBy)
        {
            try
            {
                //ExpectedConditions has been deprecated.
                //another approach is to rely on additional plugin SeleniumExtras.WaitHelpers.ExpectedConditions
                WebDriverWait wait = new WebDriverWait(appiumDriver, TimeSpan.FromSeconds(30));
                wait.Until(c => c.FindElement(mobileBy).Displayed);
            }
            catch(Exception e)
            {
                //Console.WriteLine(e.Message);
                throw new Exception("EXCEPTION - Could not find Native element - "+e);
            }
        }
        public void waitForWebElement(By webBy)
        {
            try
            {
                //ExpectedConditions has been deprecated.
                //another approach is to rely on additional plugin SeleniumExtras.WaitHelpers.ExpectedConditions
                WebDriverWait wait = new WebDriverWait(appiumDriver, TimeSpan.FromSeconds(30));
                wait.Until(c => c.FindElement(webBy).Displayed);
            }
            catch (Exception e)
            {
                throw new Exception("EXCEPTION - Could not find Web element - "+e);
                //Console.WriteLine(e.Message);
            }
        }
        public void scrollToElementInWebView(IWebElement webElement)
        {
            //Console.WriteLine("Scrolling....");
            Actions actions = new Actions(appiumDriver);
            //actions.KeyDown(Keys.Control).SendKeys(Keys.End).Perform();
            actions.MoveToElement(webElement);
            actions.Perform();

            //IJavaScriptExecutor jse = (IJavaScriptExecutor)appiumDriver;
            //jse.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
            //jse.ExecuteScript("arguments[0].scrollIntoView();", appiumDriver.FindElement(By.CssSelector("#navbarSupportedContent")));
            //Console.WriteLine("Scrolling done!");
        }
        public void scrollToElementInNativeView(AndroidElement androidElement)
        {
            Console.WriteLine("Scrolling....");
            TouchActions actions = new TouchActions(appiumDriver);
            actions.MoveToElement(androidElement);
            actions.Perform();
            Console.WriteLine("Scrolling done!");
        }
    }
}
