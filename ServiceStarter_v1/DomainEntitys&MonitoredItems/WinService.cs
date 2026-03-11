using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStarter_v1.Main;

namespace ServiceStarter_v1.DomainEntitys_MonitoredItems
{
    internal class WinService : DomainEntity
    {
        public WinService(string name, int maxRetries, int restartTimeout) : base(name, maxRetries, restartTimeout)
        {
        }

        public override ExecutionResult IsHealthy()
        {
            throw new NotImplementedException();
        }

        public override Task<ExecutionResult> RecoverAsync()
        {
            throw new NotImplementedException();
        }

        public override ExecutionResult StartAsync()
        {
            throw new NotImplementedException();
        }

        public override ExecutionResult Stop()
        {
            throw new NotImplementedException();
        }
    }
}
