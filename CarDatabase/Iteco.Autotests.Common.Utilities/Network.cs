using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Iteco.Autotests.Common.Utilities
{
    public static class Network
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct UseInfo2
        {
            internal String local;
            internal String remote;
            internal String password;
            internal UInt32 status;
            internal UInt32 asgType;
            internal UInt32 refCount;
            internal UInt32 useCount;
            internal String userName;
            internal String domainName;
        }

        [DllImport("NetApi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern UInt32 NetUseAdd(String uncServerName, UInt32 level, ref UseInfo2 buf, out UInt32 paramErrorIndex);

        public static void AuthorizeNetworkFolderAccess(string remotePath, string userName, string password, string domain)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(remotePath));
            Contract.Requires(!string.IsNullOrWhiteSpace(userName));
            
            var match = Regex.Match(remotePath, @"^(?<remote>\\\\[^\\]+\\[^\\]+)");
            
            if (!match.Success)
            {
                throw new Exception(string.Format("Bad network folder name \"{0}\"", remotePath));
            }

            var remoteName = match.Groups["remote"].Value;
            var useInfo = new UseInfo2
                {
                    local = null,
                    remote = remoteName,
                    password = password,
                    asgType = 0,
                    useCount = 1,
                    userName = userName,
                    domainName = domain
                };
            uint paramErrorIndex;
            var returnCode = NetUseAdd(null, 2, ref useInfo, out paramErrorIndex);

            const int multipleConnections = 1219;

            if (returnCode != 0 && returnCode != multipleConnections)
            {
                throw new Win32Exception((int)returnCode);
            }
        }
    }
}
