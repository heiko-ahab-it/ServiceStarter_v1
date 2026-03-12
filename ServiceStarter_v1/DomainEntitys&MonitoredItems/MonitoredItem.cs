using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ServiceStarter_v1.Main;

namespace ServiceStarter_v1.DomainEntitys_MonitoredItems
{
    internal class MonitoredItem
    {
        public DomainEntity _domainEntity { private set; get; }
        private bool IsRecovering { get; set; }
        public event Action<ExecutionResult>? RecoveryFinished;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private Task<ExecutionResult>? _recoveryTask;
        public ILogger _logger { private set; get; }

        public MonitoredItem(DomainEntity domainEntity, ILogger<MonitoredItem> logger)
        {
            _domainEntity = domainEntity;
            IsRecovering = false;
            _logger = logger;
        }
        public ExecutionResult Execute(CancellationToken token)
        {
            if( _recoveryTask != null && ! _recoveryTask.IsCompleted ) { return new ExecutionResult(false,$"{this.ToString()} is still Recovering"); }
            if (_recoveryTask != null && _recoveryTask.IsCompleted)
            {
                ExecutionResult recoveryResult = _recoveryTask.Result;
                _recoveryTask = null;
                return recoveryResult;
            }
            ExecutionResult result = _domainEntity.IsHealthy();
            if (result.IsSuccessfull) { return new ExecutionResult(result.IsSuccessfull, $"{this.ToString()}: Is Healthy : {result.Message}"); }
            _recoveryTask = PerformRecoveryAsync(token);
            return new ExecutionResult(false, $"{this.ToString()}: Recovery started ");

        }
        private async Task<ExecutionResult> PerformRecoveryAsync(CancellationToken token)
        {
            try
            {
                if (token.IsCancellationRequested) { return new ExecutionResult(false, $"Cancellation Requested"); }
                ExecutionResult result = await Task.Run(() => this._domainEntity.Recover());
                return result;
            }catch( OperationCanceledException err){ return new ExecutionResult(false, $"Recovery was cancelled -- {err}"); }
            catch(Exception ex) {return new ExecutionResult(false, $"Recovery failed: {ex.Message}");
            }
        }

        



        public override String ToString()
        {
            string result = $" Class: {this.GetType().Name}, {this._domainEntity.Name}";
            return result ;

        }

       /* public async ExecutionResult ExecuteAsync(CancellationToken token)
        {
            await _semaphore.WaitAsync(token);
            try
            {
                this._logger.LogTrace($"Processing : {this._domainEntity.Name}");
                if (IsRecovering) { return new ExecutionResult(false, $"{this}:{this._domainEntity.Name} is Recovering"); }
                ExecutionResult result = this._domainEntity.IsHealthy();
                if (result.IsSuccessfull) { return new ExecutionResult(true, $"{this._domainEntity.Name}:{result.Message}"); }
                IsRecovering = true;
                StartRecoveryBackgroundProcess(token);
                return result;
            }
            finally
            {
                _semaphore.Release();
            }
        }
        private void StartRecoveryBackgroundProcess(CancellationToken cancellationToken)
        {
        }*/
    }
}
