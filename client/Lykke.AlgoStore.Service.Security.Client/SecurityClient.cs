using System;
using Common.Log;

namespace Lykke.AlgoStore.Service.Security.Client
{
    public class SecurityClient : ISecurityClient, IDisposable
    {
        private readonly ILog _log;

        public SecurityClient(string serviceUrl, ILog log)
        {
            _log = log;
        }

        public void Dispose()
        {
            //if (_service == null)
            //    return;
            //_service.Dispose();
            //_service = null;
        }
    }
}
