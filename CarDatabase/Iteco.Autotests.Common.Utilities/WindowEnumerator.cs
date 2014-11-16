using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Iteco.Autotests.Common.Utilities
{
    public static class FastWindowEnumerator
    {
        const int MaxTitleLength = 255;
        private static List<string> _titles;
        private delegate bool EnumDelegate(IntPtr hWnd, int lParam);

        [DllImport("user32.dll", EntryPoint = "EnumDesktopWindows", ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool EnumDesktopWindows(IntPtr hDesktop, EnumDelegate lpEnumCallbackFunction, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "GetWindowText", ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int _GetWindowText(IntPtr hWnd, StringBuilder lpWindowText, int nMaxCount);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindowVisible(IntPtr hWnd);

        public static string[] WindowsTitles
        {
            get
            {
                _titles = new List<string>();

                var enumDelegate = new EnumDelegate(EnumWindowsProc);
                var successful = EnumDesktopWindows(IntPtr.Zero, enumDelegate, IntPtr.Zero);

                if (successful)
                {
                    return _titles.ToArray();
                }

                var errorCode = Marshal.GetLastWin32Error();
                var message = String.Format("EnumDesktopWindows failed with code {0}.", errorCode);

                throw new Exception(message);
            }
        }

        private static bool EnumWindowsProc(IntPtr hWnd, int lParam)
        {
            var windowText = GetWindowText(hWnd);

            if (windowText != "" & IsWindowVisible(hWnd))
            {
                _titles.Add(windowText);
            }

            return true;
        }

        private static string GetWindowText(IntPtr hWnd)
        {
            var title = new StringBuilder(MaxTitleLength);

            title.Length = _GetWindowText(hWnd, title, title.Capacity + 1);

            return title.ToString();
        }
    }
}