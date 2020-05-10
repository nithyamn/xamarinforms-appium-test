using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using System.Xml;

namespace SimpleApp.Appium.Core
{
    [SetUpFixture]
    public abstract class AppiumTest<T, W>
        where T : AppiumDriver<W>
        where W : IWebElement
    {

        protected AppiumTest(string profile, string device)
        {
            this.device = device;
            this.profile = profile;

            appiumOptions = new AppiumOptions();
        }

        protected AppiumTest()
        {
        }

        [OneTimeSetUp]
        public void OneTimeSetup()
        {

            XmlDocument doc = new XmlDocument();
            doc.Load("App.config");

            XmlNode caps = doc.DocumentElement.SelectSingleNode("/configuration/capabilities/" + profile);
            XmlNode devices = doc.DocumentElement.SelectSingleNode("/configuration/environments/" + device);
            string appId = null;

            foreach (XmlElement element in caps.ChildNodes)
            {
                appiumOptions.AddAdditionalCapability(element.GetAttribute("key"), element.GetAttribute("value"));
            }

            foreach (XmlElement element in devices.ChildNodes)
            {
                appiumOptions.AddAdditionalCapability(element.GetAttribute("key"), element.GetAttribute("value"));
            }

            username = Environment.GetEnvironmentVariable("BROWSERSTACK_USERNAME");
            accesskey = Environment.GetEnvironmentVariable("BROWSERSTACK_ACCESS_KEY");

            if (username == null || accesskey == null)
            {
                XmlNode appSettings = doc.DocumentElement.SelectSingleNode("/configuration/appSettings");
                foreach (XmlElement element in appSettings.ChildNodes)
                {
                    if (username != null && element.GetAttribute("key").Equals("user"))
                    {
                        username = element.GetAttribute("value");
                    }
                    else
                    if (accesskey != null && element.GetAttribute("key").Equals("key"))
                    {
                        accesskey = element.GetAttribute("value");
                    }
                }
            }
           

            appiumOptions.AddAdditionalCapability("browserstack.user", username);
            appiumOptions.AddAdditionalCapability("browserstack.key", accesskey);

            appId = Environment.GetEnvironmentVariable("BROWSERSTACK_APP_ID");
            if (appId != null)
            {
                appiumOptions.AddAdditionalCapability("app", appId);
            }
            driverUri = new Uri("http://" + username + ":" + accesskey + "@hub-cloud.browserstack.com/wd/hub");
            appiumDriver = GetDriver();
        }

        [OneTimeTearDown()]
        public void TearDown()
        {
            new SessionStatus(appiumDriver.SessionId.ToString()).changeSessionStatus(TestContext.CurrentContext.Result.Outcome.Status.ToString(),
                TestContext.CurrentContext.Result.Message, username, accesskey);
            appiumDriver.Quit();
        }

        protected abstract T GetDriver();
        protected abstract void InitAppiumOptions(AppiumOptions appiumOptions);
        protected T appiumDriver;
        string username = null;
        string accesskey = null;
        private string device;
        private string profile;
        protected readonly AppiumOptions appiumOptions;
        protected Uri driverUri;

        public string GetElementText(string elementId)
        {
            var element = appiumDriver.FindElement(MobileBy.AccessibilityId(elementId));
            var attributName = IsAndroid ? "text" : "value";
            return element.GetAttribute(attributName);
        }

        public bool IsAndroid => appiumDriver.Capabilities.GetCapability(MobileCapabilityType.PlatformName).Equals("Android");
    }


}