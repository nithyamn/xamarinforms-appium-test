using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.iOS;

namespace SimpleApp.Appium.Core
{
    [TestFixture("parallel_ios", "iphone-7")]
    [TestFixture("parallel_ios", "iphone-7-plus")]
    [TestFixture("parallel_ios", "iphone-11")]
    [TestFixture("parallel_ios", "iphone-xs")]
    [Parallelizable(ParallelScope.Fixtures)]
    public class TestMainPage_iOS: TestMainPage<IOSDriver<IOSElement>, IOSElement>
    {
        public TestMainPage_iOS(string profile, string device) : base(profile, device) { }
        

        protected override IOSDriver<IOSElement> GetDriver()
        {
            return new IOSDriver<IOSElement>(driverUri, appiumOptions);
        }
    }
}
