using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using NUnit.Framework;
using OpenQA.Selenium.Appium.Enums;

namespace SimpleApp.Appium.Core
{
    [TestFixture("parallel_android", "galaxy-s6")]
    [TestFixture("parallel_android", "galaxy-s7")]
    [TestFixture("parallel_android", "galaxy-s20")]
    [TestFixture("parallel_android", "pixel-3xl")]
    [TestFixture("parallel_android", "pixel")]
    [Parallelizable(ParallelScope.Fixtures)]
    public class TestMainPage_Droid: TestMainPage<AndroidDriver<AndroidElement>, AndroidElement>
    {
        public TestMainPage_Droid(string profile, string device) : base(profile, device) {}

        public object ConfigurationManager { get; }

        protected override AndroidDriver<AndroidElement> GetDriver()
        {
           return new AndroidDriver<AndroidElement>(driverUri, appiumOptions);
        }
    }
}