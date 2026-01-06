using Microsoft.Playwright;

using PageObjectModelPW.testcases;
using PageObjectModelPW.utilities;

namespace PageObjectModelPW.TestCases
{
    public class keyWordDriven
    {
        private readonly IPage _page;

        public keyWordDriven(IPage page1)
        {
            _page = page1;
        }

        public async Task Type(string pageName, string locatorName, string value)
        {
            BaseTest.test.Info("Getting text from the element : " + locatorName + " entered the value as : " + value);
            await _page.Locator(XMLLocatorReader.GetLocatorValue(pageName, locatorName)).FillAsync(value);
        }

        public async Task Click(string pageName, string locatorName)
        {
            BaseTest.test.Info("Clicking on an element : " + locatorName);
            await _page.Locator(XMLLocatorReader.GetLocatorValue(pageName, locatorName)).ClickAsync();
        }

        public async Task MouseOver(string pageName, string locatorName)
        {
            BaseTest.test.Info("Hovering over element : " + locatorName);
            await _page.HoverAsync(XMLLocatorReader.GetLocatorValue(pageName, locatorName));
        }

        public async Task<string> GetText(string pageName, string locatorName)
        {
            BaseTest.test.Info("Getting text from the element: " + locatorName);
            return await _page.Locator(XMLLocatorReader.GetLocatorValue(pageName, locatorName)).InnerTextAsync();
        }

        public async Task<bool> IsVisible(string pageName, string locatorName)
        {
            BaseTest.test.Info("Checking visibility of element: " + locatorName);
            return await _page.Locator(XMLLocatorReader.GetLocatorValue(pageName, locatorName)).IsVisibleAsync();
        }

        public async Task SelectDropdownByValue(string pageName, string locatorName, string value)
        {
            BaseTest.test.Info($"Selecting value '{value}' from dropdown: {locatorName}");
            await _page.Locator(XMLLocatorReader.GetLocatorValue(pageName, locatorName)).SelectOptionAsync(new SelectOptionValue { Value = value });
        }

        public async Task WaitForElement(string pageName, string locatorName, int timeoutMs = 5000)
        {
            BaseTest.test.Info($"Waiting for element: {locatorName}");
            await _page.Locator(XMLLocatorReader.GetLocatorValue(pageName, locatorName)).WaitForAsync(new LocatorWaitForOptions { Timeout = timeoutMs });
        }

        public async Task UploadFile(string pageName, string locatorName, string filePath)
        {
            BaseTest.test.Info($"Uploading file '{filePath}' to element: {locatorName}");
            await _page.Locator(XMLLocatorReader.GetLocatorValue(pageName, locatorName)).SetInputFilesAsync(filePath);
        }

        public async Task Clear(string pageName, string locatorName)
        {
            BaseTest.test.Info("Clearing value of element: " + locatorName);
            await _page.Locator(XMLLocatorReader.GetLocatorValue(pageName, locatorName)).FillAsync(string.Empty);
        }
    }
}
