using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using ServiceStarter_v1.Main;

namespace ServiceStarter_v1.DomainEntitys_MonitoredItems
{
    internal class WinService : DomainEntity
    {
        private ServiceController _service;
        private String TechnicalName { get; set; }
        public WinService(string name, int maxRetries, int restartTimeout, string technicalName) : base(name, maxRetries, restartTimeout)
        {
            if (!OperatingSystem.IsWindows()) { throw new PlatformNotSupportedException($"{this.GetType().Name} can only by instantiated on Windows-Devices"); }
            TechnicalName = technicalName;
            try
            {
                _service = new ServiceController(TechnicalName);

            }catch (ArgumentException) { throw; }

            

        }

        public override ExecutionResult IsHealthy()
        {
            throw new NotImplementedException();
        }

        
        public override ExecutionResult Recover()
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
