using System;
using System.Configuration;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Iteco.Autotests.Common.Utilities
{
    public abstract class AppConfig : IDisposable
    {
        public static AppConfig Change(string path)
        {
            Contract.Requires(File.Exists(path));

            return new ChangeAppConfig(path);
        }

        public abstract void Dispose();

        private class ChangeAppConfig : AppConfig
        {
            private const string Name = "APP_CONFIG_FILE";
            private readonly string _config = AppDomain.CurrentDomain.GetData(Name).ToString();
            private bool _disposedValue;

            public ChangeAppConfig(string path)
            {
                AppDomain.CurrentDomain.SetData(Name, path);
                ResetConfigMechanism();
            }

            public override void Dispose()
            {
                if (!_disposedValue)
                {
                    AppDomain.CurrentDomain.SetData(Name, _config);
                    ResetConfigMechanism();
                    _disposedValue = true;
                }

                GC.SuppressFinalize(this);
            }

            private static void ResetConfigMechanism()
            {
                // ReSharper disable PossibleNullReferenceException
                typeof(ConfigurationManager).GetField("s_initState", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, 0);
                typeof(ConfigurationManager).GetField("s_configSystem", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, null);
                typeof(ConfigurationManager).Assembly.GetTypes().First(x => x.FullName == "System.Configuration.ClientConfigPaths")
                    .GetField("s_current", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, null);
                // ReSharper restore PossibleNullReferenceException
            }
        }
    }
}