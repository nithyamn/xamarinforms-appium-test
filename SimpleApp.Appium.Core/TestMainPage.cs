using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;

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
            appiumDriver.FindElement(MobileBy.AccessibilityId("About")).Click(); // works for iOS
        }
    }
}
