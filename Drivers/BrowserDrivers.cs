using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Configuration;
using BoDi;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using TechTalk.SpecFlow;

namespace BDD_ExtentReports_NewTours.Drivers
{
    [Binding]
    public class BrowserDrivers : IDisposable
    {
        private readonly IObjectContainer _container;
        private readonly Lazy<IWebDriver> _currentWebDriver;
        private string _browserDriverToUse;

        public static ExtentReports _extentReports;
        public static ExtentTest _feature;
        public static ExtentTest _scenario;

        //public static string dir = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string curentDateTime = DateTime.Now.ToString("ddMMyyyy_hhmmss");
        public static string dir = AppDomain.CurrentDomain.BaseDirectory;
        public static string testResultFolder = dir.Replace("bin\\Debug", "TestResults");
        public static string testOutputFolder = testResultFolder + curentDateTime + "\\";
        
        public BrowserDrivers(IObjectContainer container)
        {
            _container = container;
            _currentWebDriver = new Lazy<IWebDriver>(CreateWebDriver);
            Directory.CreateDirectory(testOutputFolder);
        }

        public IWebDriver Current => _currentWebDriver.Value;

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            Console.WriteLine("Running before test run...");
            InitExtentReport();
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            Console.WriteLine("Running after test run...");
            ExtentReportTearDown();
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            Console.WriteLine("Running before feature...");
            _feature = _extentReports.CreateTest<Feature>(featureContext.FeatureInfo.Title);
        }

        [AfterFeature]
        public static void AfterFeature()
        {
            Console.WriteLine("Running after feature...");
        }

        [BeforeScenario]
        public void FirstBeforeScenario(ScenarioContext scenarioContext)
        {
            Console.WriteLine("Running before scenario...");

            _container.RegisterInstanceAs<IWebDriver>(Current);

            _scenario = _feature.CreateNode<Scenario>(scenarioContext.ScenarioInfo.Title);
        }

        [AfterScenario]
        public void AfterScenario()
        {
            Console.WriteLine("Running after scenario...");
            var driver = _container.Resolve<IWebDriver>();

            if (driver != null)
            {
                driver.Quit();
            }
        }

        [AfterStep]
        public void AfterStep(ScenarioContext scenarioContext)
        {
            Console.WriteLine("Running after step....");
            string stepType = scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
            string stepName = scenarioContext.StepContext.StepInfo.Text;

            var driver = _container.Resolve<IWebDriver>();

            //When scenario passed
            if (scenarioContext.TestError == null)
            {
                if (stepType == "Given")
                {
                    _scenario.CreateNode<Given>(stepName);
                }
                else if (stepType == "When")
                {
                    _scenario.CreateNode<When>(stepName);
                }
                else if (stepType == "Then")
                {
                    _scenario.CreateNode<Then>(stepName);
                }
                else if (stepType == "And")
                {
                    _scenario.CreateNode<And>(stepName);
                }
            }

            //When scenario fails
            if (scenarioContext.TestError != null)
            {

                if (stepType == "Given")
                {
                    _scenario.CreateNode<Given>(stepName).Fail(scenarioContext.TestError.Message,
                        MediaEntityBuilder.CreateScreenCaptureFromPath(AddScreenshot(driver, scenarioContext)).Build());
                }
                else if (stepType == "When")
                {
                    _scenario.CreateNode<When>(stepName).Fail(scenarioContext.TestError.Message,
                        MediaEntityBuilder.CreateScreenCaptureFromPath(AddScreenshot(driver, scenarioContext)).Build());
                }
                else if (stepType == "Then")
                {
                    _scenario.CreateNode<Then>(stepName).Fail(scenarioContext.TestError.Message,
                        MediaEntityBuilder.CreateScreenCaptureFromPath(AddScreenshot(driver, scenarioContext)).Build());
                }
                else if (stepType == "And")
                {
                    _scenario.CreateNode<And>(stepName).Fail(scenarioContext.TestError.Message,
                        MediaEntityBuilder.CreateScreenCaptureFromPath(AddScreenshot(driver, scenarioContext)).Build());
                }
            }
        }

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

        public static void InitExtentReport()
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

        public static void ExtentReportTearDown()
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
