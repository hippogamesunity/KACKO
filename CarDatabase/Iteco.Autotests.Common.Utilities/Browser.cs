using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;

namespace Iteco.Autotests.Common.Utilities
{
    [Serializable]
    public enum Browsers
    {
        Default,

        [Description("Windows Internet Explorer")]
        InternetExplorer,

        [Description("Mozilla Firefox")]
        Firefox,

        [Description("Google Chrome")]
        Chrome
    }

    public static class Browser
    {
        #region Public properties

        public static Browsers SelectedBrowser { get; set; }

        public static Uri Url
        {
            get { WaitAjax(); return new Uri(WebDriver.Url); }
        }

        public static string Title
        {
            get
            {
                WaitAjax();
                return string.Format("{0} - {1}", WebDriver.Title, EnumHelper.GetEnumDescription(SelectedBrowser));
            }
        }

        public static string PageSource
        {
            get { WaitAjax(); return WebDriver.PageSource; }
        }

        public static bool IsStarted
        {
            get { return _webDriver != null; }
        }

        public static string DownloadsFolder
        {
            get
            {
                switch (SelectedBrowser)
                {
                    case Browsers.Chrome:
                        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        #endregion

        #region Public methods

        public static void Start()
        {
            _webDriver = StartWebDriver();
        }

        public static void Navigate(Uri url)
        {
            Contract.Requires(url != null);

            WebDriver.Navigate().GoToUrl(url);
        }

        public static void Dispose()
        {
            if (_webDriver == null) return;

            try
            {
                _webDriver.Close();
                _webDriver.Quit();
            }
            finally
            {
                _webDriver = null;
            }
        }

        public static void WaitReadyState()
        {
            Contract.Assume(WebDriver != null);

            var ready = new Func<bool>(() => (bool) ExecuteJavaScript("return document.readyState == 'complete'"));

            Contract.Assert(Executor.SpinWait(ready, TimeSpan.FromSeconds(60), TimeSpan.FromMilliseconds(100)));
        }

        public static void WaitAjax()
        {
            Contract.Assume(WebDriver != null);

            var ready = Executor.SpinWait(() => (bool) ExecuteJavaScript("return (typeof($) === 'undefined') ? true : !$.active;"),
                TimeSpan.FromSeconds(100), TimeSpan.FromMilliseconds(500));

            if (!ready)
            {
                SaveScreenshot(string.Format(@"c:\WaitAjax_{0}.jpeg", RandomHelper.RandomString));
            }

            Contract.Assert(ready, "$.active = true (processing ajax request)");
        }

        public static int WindowsCount
        {
            get { return WebDriver.WindowHandles.Count; }
        }

        public static void SwitchToFrame(IWebElement inlineFrame)
        {
            WebDriver.SwitchTo().Frame(inlineFrame);
        }

        public static void SwitchToPopupWindow()
        {
            var windows = WebDriver.WindowHandles.Where(handle => handle != _mainWindowHandler).ToList();

            Contract.Assert(windows.Count == 1);

            WebDriver.SwitchTo().Window(windows[0]);
        }

        public static void ClosePopupWindow()
        {
            var windows = WebDriver.WindowHandles.Where(handle => handle != _mainWindowHandler).ToList(); //TODO: cp

            Contract.Assert(windows.Count == 1);

            WebDriver.SwitchTo().Window(windows[0]).Close();
        }

        public static void SwitchToMainWindow()
        {
            WebDriver.SwitchTo().Window(_mainWindowHandler);
        }

        public static void SwitchToDefaultContent()
        {
            WebDriver.SwitchTo().DefaultContent();
        }

        public static void AcceptAlert()
        {
            var accept = Executor.MakeTry(() => WebDriver.SwitchTo().Alert().Accept());

            Executor.SpinWait(accept, TimeSpan.FromSeconds(5));
        }

        public static IEnumerable<IWebElement> FindElements(By selector)
        {
            Contract.Assume(WebDriver != null);

            return WebDriver.FindElements(selector);
        }

        public static Screenshot GetScreenshot()
        {
            WaitReadyState();

            return ((ITakesScreenshot)WebDriver).GetScreenshot();
        }

        public static void SaveScreenshot(string path)
        {
            Contract.Requires(!string.IsNullOrEmpty(path));

            GetScreenshot().SaveAsFile(path, ImageFormat.Jpeg);

            Console.WriteLine("Screenshot of webpage [{0}] saved as: {1}.", WebDriver.Url ?? "?", path);
        }

        public static void DragAndDrop(IWebElement source, IWebElement destination)
        {
            (new Actions(WebDriver)).DragAndDrop(source, destination).Build().Perform();
        }

        public static void ResizeWindow(int width, int height)
        {
            ExecuteJavaScript(string.Format("window.resizeTo({0}, {1});", width, height));
        }

        public static void NavigateBack()
        {
            WebDriver.Navigate().Back();
        }

        public static void Refresh()
        {
            WebDriver.Navigate().Refresh();
        }

        public static object ExecuteJavaScript(string javaScript, params object[] args)
        {
            var javaScriptExecutor = (IJavaScriptExecutor) WebDriver;

            return javaScriptExecutor.ExecuteScript(javaScript, args);
        }

        public static void KeyDown(string key)
        {
            new Actions(WebDriver).KeyDown(key);
        }

        public static void KeyUp(string key)
        {
            new Actions(WebDriver).KeyUp(key);
        }

        public static void AlertAccept()
        {
            Thread.Sleep(2000);
            WebDriver.SwitchTo().Alert().Accept();
            WebDriver.SwitchTo().DefaultContent();
        }

        #endregion

        #region Private

        private static IWebDriver _webDriver;
        private static string _mainWindowHandler;

        private static IWebDriver WebDriver
        {
            get { return _webDriver ?? StartWebDriver(); }
        }

        private static IWebDriver StartWebDriver()
        {
            Contract.Ensures(Contract.Result<IWebDriver>() != null);

            if (_webDriver != null) return _webDriver;

            switch (SelectedBrowser)
            {
                case Browsers.Default:
                case Browsers.Chrome:
                    _webDriver = StartChrome();
                    break;
                case Browsers.InternetExplorer:
                    _webDriver = StartInternetExplorer();
                    break;
                case Browsers.Firefox:
                    _webDriver = StartFirefox();
                    break;
                default:
                    throw new Exception(string.Format("Unknown browser selected: {0}.", SelectedBrowser));
            }

            _webDriver.Manage().Window.Maximize();
            _mainWindowHandler = _webDriver.CurrentWindowHandle;

            return WebDriver;
        }

        private static InternetExplorerDriver StartInternetExplorer()
        {
            var internetExplorerOptions = new InternetExplorerOptions
            {
                IntroduceInstabilityByIgnoringProtectedModeSettings = true,
                InitialBrowserUrl = "about:blank",
                EnableNativeEvents = true
            };

            return new InternetExplorerDriver(Directory.GetCurrentDirectory(), internetExplorerOptions);
        }

        private static FirefoxDriver StartFirefox()
        {
            var firefoxProfile = new FirefoxProfile
            {
                AcceptUntrustedCertificates = true,
                EnableNativeEvents = true
            };

            return new FirefoxDriver(firefoxProfile);
        }

        private static ChromeDriver StartChrome()
        {
            var chromeOptions = new ChromeOptions();

            chromeOptions.AddArgument("test-type");

            var defaultDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
                + @"\..\Local\Google\Chrome\User Data\Default";

            if (Directory.Exists(defaultDataFolder))
            {
                Executor.Try(() => DirectoryExtension.ForceDelete(defaultDataFolder));
            }

            return new ChromeDriver(chromeOptions);
        }

        #endregion
    }
}
