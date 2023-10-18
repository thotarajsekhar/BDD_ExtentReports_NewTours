Feature: NewToursHomePageValidation
	Validation for New Tours Home Page

@smokeTest
Scenario: Verify Landing in Guru Demo Site
	Given I open browser and navigate to New Tours Website
	When I click on the New Tours Link 
	Then I Verify the title of the website
	And I close the Browser

@regression
Scenario: Login to the system 
	Given I open browser and navigate to New Tours Website
	When I login with "Username" and "Password"
	Then I verify I will be logged in to the System
	And I close the Browser