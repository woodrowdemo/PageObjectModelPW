Feature: SauceLogin Page

This is to test the login page for sauce demo page


Scenario: Testing Logging into the SauceDemo Page with invalid credentials
	Given I navigate to the SauceDemo login page
	When I enter invalid username and password
	Then I should see an error message indicating invalid credentials