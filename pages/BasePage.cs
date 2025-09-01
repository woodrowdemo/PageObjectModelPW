using Microsoft.Playwright;

namespace PageObjectModelPW.pages
{
    /// <summary>
    /// Base class for the Page Object Model pattern implementation using Playwright.
    /// Provides common functionality and properties for all page objects in the test automation framework.
    /// </summary>
    public class BasePage
    {
        /// <summary>
        /// The Playwright IPage instance that represents the web page being automated.
        /// This is used by derived classes to interact with web elements.
        /// </summary>
        public IPage page;

        /// <summary>
        /// Initializes a new instance of the BasePage class.
        /// </summary>
        /// <param name="page">The Playwright IPage instance to be used for web automation.</param>
        public BasePage(IPage page)
        {
            this.page = page;
        }
    }
}
