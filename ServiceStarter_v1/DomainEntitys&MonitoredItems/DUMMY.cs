using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStarter_v1.Main;

namespace ServiceStarter_v1.DomainEntitys_MonitoredItems
{
    internal class DUMMY : DomainEntity
    {
        private ILogger _logger;
        public DUMMY(string name, int maxRetries, int recoveryTimeout, ILogger<DUMMY> logger) : base(name, maxRetries, recoveryTimeout)
        {
            this._logger = logger;
        }
        private bool getRandom()
        {
            bool randomSuccess = new Random().Next(3) == 1;
            return randomSuccess;

        }

        public override ExecutionResult IsHealthy()
        {
            bool randomSuccess = new Random().Next(4) != 1;
            return new ExecutionResult(randomSuccess, randomSuccess == true ? "ServiceStart ist gesund" : "Service ist unerwartet gestoppt");
        }

        public override async Task<ExecutionResult> RecoverAsync()
        {
            await Task.Delay(1000);  // stellvetretend für recovery(also neustart des dienstes)
            bool randomSuccess = new Random().Next(2) == 1;
            ExecutionResult result = new ExecutionResult(randomSuccess, randomSuccess == true ? "Service RECOVERY war erfolgreich" : "Service RECOVERY Fehlgeschlagen");

            return result;
        }

        public override ExecutionResult StartAsync()
        {
            bool randomSuccess = new Random().Next(4) != 1;
            return new ExecutionResult(randomSuccess, randomSuccess == true ? "ServiceStart war erfolgreich" : "Service konnte nicht gestartet werden");
        }

        public override ExecutionResult Stop()
        {
            bool randomSuccess = new Random().Next(2) == 1;
            return new ExecutionResult(randomSuccess, randomSuccess == true ? "Service erfolgreich heruntergefahren" : "Service konnte nicht gestoppt werden");

        }
    }
}
