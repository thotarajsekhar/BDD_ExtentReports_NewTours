using BDD_ExtentReports_NewTours.Drivers;
using BDD_ExtentReports_NewTours.PageObjects;
using TechTalk.SpecFlow;

namespace BDD_ExtentReports_NewTours.FeaturesStepDefinition
{
    [Binding]
    public class NewToursHomePageValidationSteps
    {

        private readonly HomePageObjects _homePageObjects;
        private readonly BrowserDrivers _browserDrivers;

        public NewToursHomePageValidationSteps(BrowserDrivers browserDriver)
        {
            _browserDrivers = browserDriver;
            _homePageObjects = new HomePageObjects(_browserDrivers.Current);
        }
        [Given(@"I open browser and navigate to New Tours Website")]
        public void GivenIOpenBrowserAndNavigateToNewToursWebsite()
        {
            
        }
        
        [When(@"I click on the New Tours Link")]
        public void WhenIClickOnTheNewToursLink()
        {
            _homePageObjects.ClickNewToursLink();
        }
        
        [Then(@"I Verify the title of the website")]
        public void ThenIVerifyTheTitleOfTheWebsite()
        {
            _homePageObjects.VerifyTitleOfPage(_homePageObjects._driver);
        }
        
        [Then(@"I close the Browser")]
        public void ThenICloseTheBrowser()
        {
            _browserDrivers.Dispose();
            _browserDrivers.ExtentReportFlush();
        }
    }
}
