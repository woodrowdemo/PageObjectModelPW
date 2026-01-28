using Microsoft.Playwright;

using PageObjectModelPW.testcases;
using PageObjectModelPW.utilities;

/*
 KeyWordDriven — Provides keyword-style wrapper methods around Playwright's IPage:
 - Actions: Type, Click, MouseOver, GetText, IsVisible, SelectDropdownByValue, WaitForElement, UploadFile, Clear
 - Resolves locators via `XMLLocatorReader.GetLocatorValue(pageName, locatorName)`
 - Logs actions to `BaseTest.test` for reporting
*/

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
            BaseTest.test.Info("Typing into element : " + locatorName + " value: " + value);
            var locator = await LocatorResolver.ResolveAsync(_page, pageName, locatorName);
            if (locator == null)
            {
                // include diagnostic info in exception
                List<string> candidates = null;
                string xmlPath = null;
                try
                {
                    xmlPath = utilities.XMLLocatorReader.GetXmlPath();
                    candidates = utilities.XMLLocatorReader.GetLocatorValues(pageName, locatorName);
                }
                catch { }
                var candText = candidates == null || candidates.Count == 0 ? "<none>" : string.Join(" | ", candidates);
                throw new Exception($"Unable to resolve locator for {pageName}.{locatorName}. XML: {xmlPath ?? "<unknown>"}. Candidates: {candText}");
            }
            await locator.FillAsync(value);
        }

        public async Task Click(string pageName, string locatorName)
        {
            BaseTest.test.Info("Clicking on an element : " + locatorName);
            // diagnostic: log candidate selectors
            try
            {
                var xmlPath = utilities.XMLLocatorReader.GetXmlPath();
                BaseTest.test.Info($"Locators xml path: {xmlPath}");
                var candidates = utilities.XMLLocatorReader.GetLocatorValues(pageName, locatorName);
                BaseTest.test.Info($"Candidates for {pageName}.{locatorName}: {string.Join(" | ", candidates ?? new List<string>())}");
            }
            catch (Exception ex)
            {
                BaseTest.test.Warning("Failed to enumerate locator candidates: " + ex.Message);
            }

            var locator = await LocatorResolver.ResolveAsync(_page, pageName, locatorName);
            if (locator == null) throw new Exception($"Unable to resolve locator for {pageName}.{locatorName}");
            await locator.ClickAsync();
        }

        public async Task MouseOver(string pageName, string locatorName)
        {
            BaseTest.test.Info("Hovering over element : " + locatorName);
            // diagnostic: log candidates
            try
            {
                var xmlPath = utilities.XMLLocatorReader.GetXmlPath();
                BaseTest.test.Info($"Locators xml path: {xmlPath}");
                var candidates = utilities.XMLLocatorReader.GetLocatorValues(pageName, locatorName);
                BaseTest.test.Info($"Candidates for {pageName}.{locatorName}: {string.Join(" | ", candidates ?? new List<string>())}");
            }
            catch (Exception ex)
            {
                BaseTest.test.Warning("Failed to enumerate locator candidates: " + ex.Message);
            }

            var locator = await LocatorResolver.ResolveAsync(_page, pageName, locatorName);
            if (locator == null) throw new Exception($"Unable to resolve locator for {pageName}.{locatorName}");
            await locator.HoverAsync();
            Console.WriteLine("Hovered over element: " + locatorName);
            await Task.Delay(500); // Short delay to allow hover-triggered items to appear
        }

        public async Task<string> GetText(string pageName, string locatorName)
        {
            BaseTest.test.Info("Getting text from the element: " + locatorName);
            var locator = await LocatorResolver.ResolveAsync(_page, pageName, locatorName);
            if (locator == null) throw new Exception($"Unable to resolve locator for {pageName}.{locatorName}");
            return await locator.InnerTextAsync();
        }

        public async Task<bool> IsVisible(string pageName, string locatorName)
        {
            BaseTest.test.Info("Checking visibility of element: " + locatorName);
            var locator = await LocatorResolver.ResolveAsync(_page, pageName, locatorName);
            if (locator == null) return false;
            return await locator.IsVisibleAsync();
        }

        public async Task SelectDropdownByValue(string pageName, string locatorName, string value)
        {
            BaseTest.test.Info($"Selecting value '{value}' from dropdown: {locatorName}");
            var locator = await LocatorResolver.ResolveAsync(_page, pageName, locatorName);
            if (locator == null) throw new Exception($"Unable to resolve locator for {pageName}.{locatorName}");
            await locator.SelectOptionAsync(new SelectOptionValue { Value = value });
        }

        public async Task WaitForElement(string pageName, string locatorName, int timeoutMs = 5000)
        {
            BaseTest.test.Info($"Waiting for element: {locatorName}");
            var locator = await LocatorResolver.ResolveAsync(_page, pageName, locatorName);
            if (locator == null) throw new Exception($"Unable to resolve locator for {pageName}.{locatorName}");
            await locator.WaitForAsync(new LocatorWaitForOptions { Timeout = timeoutMs });
        }

        public async Task UploadFile(string pageName, string locatorName, string filePath)
        {
            BaseTest.test.Info($"Uploading file '{filePath}' to element: {locatorName}");
            var locator = await LocatorResolver.ResolveAsync(_page, pageName, locatorName);
            if (locator == null) throw new Exception($"Unable to resolve locator for {pageName}.{locatorName}");
            await locator.SetInputFilesAsync(filePath);
        }

        public async Task Clear(string pageName, string locatorName)
        {
            BaseTest.test.Info("Clearing value of element: " + locatorName);
            var locator = await LocatorResolver.ResolveAsync(_page, pageName, locatorName);
            if (locator == null) throw new Exception($"Unable to resolve locator for {pageName}.{locatorName}");
            await locator.FillAsync(string.Empty);
        }
    }
}
