using NUnit.Framework;
using OpenQA.Selenium;

namespace BDD_ExtentReports_NewTours.PageObjects
{
    public class HomePageObjects
    {
        internal readonly IWebDriver _driver;
        private readonly string Page_Title = "Welcome: Mercury Tours";
        public HomePageObjects(IWebDriver driver)
        {
            _driver = driver;
        }

        private IWebElement NewTours_Link => _driver.FindElement(By.LinkText("New Tours"));
        
        public void ClickNewToursLink()
        {
            // Click on NEw Tours Link Element
            NewTours_Link.Click();
        }

        public void VerifyTitleOfPage(IWebDriver driver)
        {
            Assert.AreEqual(Page_Title, driver.Title);
        }

    }
}
