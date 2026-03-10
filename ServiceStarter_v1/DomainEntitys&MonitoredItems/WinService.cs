using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStarter_v1.DomainEntitys_MonitoredItems
{
    internal class WinService : DomainEntity
    {
        public WinService(string name, int maxRetries, int restartTimeout) : base(name, maxRetries, restartTimeout)
        {
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
