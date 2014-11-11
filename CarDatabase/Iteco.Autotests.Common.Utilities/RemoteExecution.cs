using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Management;
using System.Net;

namespace Iteco.Autotests.Common.Utilities
{
    public class RemoteExecution
    {
        private readonly string _host;
        private readonly NetworkCredential _networkCredential;

        public RemoteExecution(string host, NetworkCredential networkCredential)
        {
            Contract.Requires(!string.IsNullOrEmpty(host));
            Contract.Requires(networkCredential != null);

            _host = host;
            _networkCredential = networkCredential;
        }

        public void ExecuteAsync(string commnand)
        {
            Contract.Requires(!string.IsNullOrEmpty(commnand));

            object[] processToRun = { commnand };
            var isLocal = Dns.GetHostName() == _host || Dns.GetHostEntry(Dns.GetHostName()).AddressList.Any(i => i.ToString().Equals(_host));
            
            var connection = isLocal
                ? new ConnectionOptions()
                : new ConnectionOptions
                {
                    Username = string.Format(@"{0}\{1}",
                        _networkCredential.Domain,
                        _networkCredential.UserName),
                    Password = _networkCredential.Password
                };

            var scope = new ManagementScope(String.Format(@"\\{0}\root\cimv2", _host), connection);
            var process = new ManagementClass(scope, new ManagementPath("Win32_Process"), new ObjectGetOptions());

            process.InvokeMethod("Create", processToRun);
        }
    }
}