using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;

namespace BDD_ExtentReports_NewTours.Drivers
{
    public class BrowserDrivers : IDisposable
    {
        private readonly Lazy<IWebDriver> _currentWebDriver;
        private string _browserDriverToUse;

        public BrowserDrivers()
        {
            _currentWebDriver = new Lazy<IWebDriver>(CreateWebDriver);
        }

        public IWebDriver Current => _currentWebDriver.Value;

        private IWebDriver CreateWebDriver()
        {
            IWebDriver webDriver;

            using (StreamReader r = new StreamReader(@".\configData.json"))
            {
                string jsonObj = r.ReadToEnd();
                dynamic array = JsonConvert.DeserializeObject(jsonObj);
                foreach (var item in array)
                {
                    _browserDriverToUse = item.BrowserName;
                }
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

    }
}
