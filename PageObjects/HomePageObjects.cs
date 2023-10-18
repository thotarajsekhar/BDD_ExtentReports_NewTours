using NUnit.Framework;
using OpenQA.Selenium;

namespace BDD_ExtentReports_NewTours.PageObjects
{
    public class HomePageObjects
    {
        internal readonly IWebDriver _driver;
        private readonly string Page_Title = "Welcome: Mercury Tours";
        private readonly string SignOff_Text = "SIGN-OFF";

        public HomePageObjects(IWebDriver driver)
        {
            _driver = driver;
        }

        private IWebElement NewTours_Link => _driver.FindElement(By.LinkText("New Tours"));

        private IWebElement Login_Text_Name => _driver.FindElement(By.Name("userName"));
        private IWebElement Password_Text_Name => _driver.FindElement(By.Name("password"));
        private IWebElement Submit_Button_Name => _driver.FindElement(By.Name("submitt"));
        private IWebElement SignOff_LinkText => _driver.FindElement(By.LinkText("SIGN-OFF"));

        public void ClickNewToursLink()
        {
            // Click on New Tours Link Element
            NewTours_Link.Click();
        }

        public void VerifyTitleOfPage(IWebDriver driver)
        {
            Assert.AreEqual(Page_Title, driver.Title);
        }

        public void EnterUserName(string userName)
        {
            Login_Text_Name.Clear();
            Login_Text_Name.SendKeys(userName);
        }

        public void EnterPassword(string password)
        {
            Password_Text_Name.Clear();
            Password_Text_Name.SendKeys(password);
        }

        public void ClickOnSubmit()
        {
            Submit_Button_Name.Click();
        }

        public void VerifySignOffLink()
        {
            Assert.AreEqual(SignOff_Text, SignOff_LinkText.Text);
        }
    }
}
