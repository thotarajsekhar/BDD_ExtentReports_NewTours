using BDD_ExtentReports_NewTours.Drivers;
using BDD_ExtentReports_NewTours.PageObjects;
using TechTalk.SpecFlow;

namespace BDD_ExtentReports_NewTours.FeaturesStepDefinition
{
    [Binding]
    public class NewToursHomePageValidationSteps
    {

        private readonly HomePageObjects _homePageObjects;

        public NewToursHomePageValidationSteps(BrowserDrivers browserDriver)
        {
            _homePageObjects = new HomePageObjects(browserDriver.Current);
        }
        [Given(@"I open browser and navigate to New Tours Website")]
        public void GivenIOpenBrowserAndNavigateToNewToursWebsite()
        {
            
        }
        
        [When(@"I click on the New Tours Link")]
        public void WhenIClickOnTheNewToursLink()
        {
        }
        
        [Then(@"I Verify the title of the website")]
        public void ThenIVerifyTheTitleOfTheWebsite()
        {
        }
        
        [Then(@"I close the Browser")]
        public void ThenICloseTheBrowser()
        {
        }
    }
}
