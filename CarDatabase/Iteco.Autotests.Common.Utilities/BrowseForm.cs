using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Forms;

namespace Iteco.Autotests.Common.Utilities
{
    public class BrowseForm
    {
        private readonly string _windowTitle;

        public BrowseForm(string windowTitle)
        {
            Contract.Assert(!string.IsNullOrEmpty(windowTitle));

            _windowTitle = windowTitle;
        }

        public string Value
        {
            set
            {
                Contract.Assert(!string.IsNullOrEmpty(value));

                Thread.Sleep(2000); // TODO: Fix

                FocusEdit(_windowTitle);
                SetBrowseForm(value);

                Thread.Sleep(2000); // TODO: Fix
            }
        }

        private static void FocusEdit(string windowTitle)
        {
            var root = AutomationElement.RootElement;

            var browserWindowSearchCondition = new PropertyCondition(AutomationElement.NameProperty, windowTitle);
            var browserWindow = root.FindFirst(TreeScope.Children, browserWindowSearchCondition);

            Contract.Assert(browserWindow != null);

            var browseWindowSearchCondition = new PropertyCondition(AutomationElement.ClassNameProperty, "#32770");
            var getBrowseWindow = new Func<AutomationElement>(() => browserWindow.FindFirst(
                TreeScope.Descendants,
                browseWindowSearchCondition));

            Contract.Assert(Executor.SpinWait(() => getBrowseWindow() != null, TimeSpan.FromSeconds(10)));
          
            getBrowseWindow().SetFocus();
        }

        private static void SetBrowseForm(string path)
        {
            SendKeys.SendWait("{BACKSPACE}");

            Thread.Sleep(2000);
            
            SendKeys.SendWait(path);

            Thread.Sleep(1000);

            SendKeys.SendWait("~");
        }
    }
}