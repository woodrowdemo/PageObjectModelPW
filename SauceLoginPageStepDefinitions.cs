using Microsoft.Playwright;

using PageObjectModelPW.testcases;

using Reqnroll;

namespace PageObjectModelPW
{
    [Binding]
    public class SauceLoginPageStepDefinitions : BaseTest
    {
        private IBrowser _browser;
        private IPage _page;

        [Given("I navigate to the SauceDemo login page")]
        public void GivenINavigateToTheSauceDemoLoginPage( )
        {

        }

        [When("I enter invalid username and password")]
        public void WhenIEnterInvalidUsernameAndPassword( )
        {

        }

        [Then("I should see an error message indicating invalid credentials")]
        public void ThenIShouldSeeAnErrorMessageIndicatingInvalidCredentials( )
        {

        }
    }
}
