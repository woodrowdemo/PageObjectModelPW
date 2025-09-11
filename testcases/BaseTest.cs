using AventStack.ExtentReports;

using log4net;

using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;

namespace PageObjectModelPW.testcases
{
    public class BaseTest
    {
        // Playwright, Extent Reports, Logs, Configuration, fix, capture shots etc.

        protected IPlaywright playwright;
        private static ExtentReports extent;
        public static ExtentTest test;

        private static readonly ILog log = LogManager.GetLogger(typeof(BaseTest));

        IConfiguration configuration;

        public void OneTimeSetUp( )
        {

        }

    }
}
