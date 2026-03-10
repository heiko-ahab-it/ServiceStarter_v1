using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigLoader_v2;

namespace ServiceStarter_v1.DomainEntitys_MonitoredItems
{
    internal class PortTest : DomainEntity
    {
        private readonly string _server;
        private readonly int _port;
        private readonly PROTOCOL _protocol;
        private readonly int _startOk;
        private readonly int _endOk;

        public PortTest(string name, int maxRetries, int recoveryTimeout,
            string server,int port,PROTOCOL protocol, int startRangeOk,int endRangeOk) : base(name, maxRetries, recoveryTimeout)
        {
            this._server = server;
            this._port = port;
            this._protocol = protocol;
            this._startOk = startRangeOk;
            this._endOk = endRangeOk;

        }

        protected override ExecutionResult IsHealthy()
        {
            throw new NotImplementedException();
        }

        protected override Task<ExecutionResult> RecoverAsync()
        {
            throw new NotImplementedException();
        }

        protected override ExecutionResult StartAsync()
        {
            throw new NotImplementedException();
        }

        protected override ExecutionResult Stop()
        {
            throw new NotImplementedException();
        }
    }
}
