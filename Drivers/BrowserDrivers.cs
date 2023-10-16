using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Configuration;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using TechTalk.SpecFlow;

namespace BDD_ExtentReports_NewTours.Drivers
{
    public class BrowserDrivers : IDisposable
    {
        private readonly Lazy<IWebDriver> _currentWebDriver;
        private string _browserDriverToUse;

        public static ExtentReports _extentReports;
        public static ExtentTest _feature;
        public static ExtentTest _scenario;

        //public static string dir = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string curentDateTime = DateTime.Now.ToString("ddMMyyyy_hhmmss");
        public static string dir = AppDomain.CurrentDomain.BaseDirectory;
        public static string testResultFolder = dir.Replace("bin\\Debug", "TestResults");
        public static string testOutputFolder = testResultFolder + curentDateTime;
        
        public BrowserDrivers()
        {
            _currentWebDriver = new Lazy<IWebDriver>(CreateWebDriver);
            Directory.CreateDirectory(testOutputFolder);
            InitExtentReport();
        }

        public IWebDriver Current => _currentWebDriver.Value;

        private IWebDriver CreateWebDriver()
        {
            IWebDriver webDriver;
            using (StreamReader r = new StreamReader(@".\configData.json"))
            {
                string jsonObj = r.ReadToEnd();
                dynamic array = JsonConvert.DeserializeObject(jsonObj);
                _browserDriverToUse = array.BrowserName;
                
                // somehow this code block is not needed. 
                //foreach (var item in array)
                //{
                //    _browserDriverToUse = item.BrowserName;
                //    break;
                //}
            }

            switch (_browserDriverToUse)
            {
                case "Chrome":
                    webDriver = new ChromeDriver();
                    break;
                default:
                    webDriver = new ChromeDriver();
                    break;
            }

            webDriver.Manage().Cookies.DeleteAllCookies();
            webDriver.Manage().Window.Maximize();
            webDriver.Url = "https://demo.guru99.com/test/newtours/";
            return webDriver;
        }

        public void Dispose()
        {
            Current.Quit();
            Current.Dispose();
        }

        public void InitExtentReport()
        {
            var _htmlReporter = new ExtentHtmlReporter(testOutputFolder);
            _htmlReporter.Config.ReportName = "Automation Test Report";
            _htmlReporter.Config.DocumentTitle = "Test Reporter";
            _htmlReporter.Config.Theme = Theme.Standard;
            _htmlReporter.Start();

            _extentReports = new ExtentReports();
            _extentReports.AttachReporter(_htmlReporter);
            _extentReports.AddSystemInfo("OS", "Windows");
            _extentReports.AddSystemInfo("Browser", "Chrome");
            _extentReports.AddSystemInfo("Application", "New Tours");
        }

        public void ExtentReportFlush()
        {
            _extentReports.Flush();
        }

        public string AddScreenshot(IWebDriver driver, ScenarioContext scenarioContext)
        {
            ITakesScreenshot takesScreenshot = (ITakesScreenshot)driver;
            Screenshot screenshot = takesScreenshot.GetScreenshot();
            string screenshotLocation = Path.Combine(testOutputFolder, scenarioContext.ScenarioInfo.Title + ".png");
            screenshot.SaveAsFile(screenshotLocation, ScreenshotImageFormat.Png);
            return screenshotLocation;
        }

    }
}
