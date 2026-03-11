using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStarter_v1.Main;

namespace ServiceStarter_v1.DomainEntitys_MonitoredItems
{
    internal class MonitoredItem
    {
        public DomainEntity _domainEntity { private set; get; }
        private bool IsRecovery {  get; set; }
        public event Action<ExecutionResult>? RecoveryFinished;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        public ILogger _logger { private set; get; }

        public MonitoredItem(DomainEntity domainEntity, ILogger<MonitoredItem> logger)
        {
            _domainEntity = domainEntity;
            IsRecovery = false;
            _logger = logger;
        }

        public ExecutionResult ExecuteAsync(CancellationToken token) { 
            _logger.LogTrace($"ExecuteAsync : {this._domainEntity.Name}");
            return new ExecutionResult(true, "ExecuteAsync");
        }
    }
}
