using System.Configuration;
using System.Diagnostics.Contracts;
using System.IO;
using System.Web.Configuration;

namespace Iteco.Autotests.Common.Utilities
{
    public static class ConfigurationReader
    {
        public static Configuration ReadWebConfiguration(string physicalDirectory)
        {
            Contract.Assert(Directory.Exists(physicalDirectory));

            var virtualDirectoryMapping = new VirtualDirectoryMapping(physicalDirectory, true);
            var webConfigurationFileMap = new WebConfigurationFileMap();

            webConfigurationFileMap.VirtualDirectories.Add("/", virtualDirectoryMapping);

            return WebConfigurationManager.OpenMappedWebConfiguration(webConfigurationFileMap, "/");
        }
    }
}
