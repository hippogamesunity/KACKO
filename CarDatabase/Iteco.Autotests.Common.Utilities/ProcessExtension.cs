using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;

namespace Iteco.Autotests.Common.Utilities
{
    public static class ProcessExtension
    {
        public static void Execute(string path, string arguments)
        {
            Execute(path, arguments, TimeSpan.FromSeconds(60));
        }
        
        public static void Execute(string path, string arguments, TimeSpan timeout)
        {
            Contract.Assert(ExecuteExitCode(path, arguments, timeout) == 0);
        }

        public static int ExecuteExitCode(string path, string arguments, TimeSpan timeout)
        {
            Contract.Requires(!string.IsNullOrEmpty(path));

            using (var process = CreateProcess(path, arguments))
            {
                process.Start();
                process.WaitForExit((int)timeout.TotalMilliseconds);

                if (!process.HasExited)
                {
                    process.Kill();
                    throw new Exception("Process not completed.");
                }

                return process.ExitCode;
            }
        }

        public static void Kill(string processName)
        {
            Process.GetProcesses().
                Where(p => p.ProcessName.Equals(processName, StringComparison.InvariantCultureIgnoreCase)).ToList().
                ForEach(p => p.Kill());
        }

        public static bool Exist(string processName)
        {
            return Process.GetProcesses().Any(p => p.ProcessName.Equals(processName, StringComparison.InvariantCultureIgnoreCase));
        }

        #region Helpers

        private static Process CreateProcess(string path, string arguments)
        {
            return new Process
                {
                    StartInfo =
                    {
                        // ReSharper disable AssignNullToNotNullAttribute
                        WorkingDirectory = Path.GetDirectoryName(path),
                        // ReSharper restore AssignNullToNotNullAttribute
                        FileName = path,
                        Arguments = arguments,
                        RedirectStandardOutput = false,
                        UseShellExecute = false
                    }
                };
        }

        #endregion
    }
}
