using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using ServiceStarter_v1.DomainEntitys_MonitoredItems;

namespace ServiceStarter_v1.Main
{
   internal  class StartUpHandler
    {
        private IDomainEntitySource _domainSource;
        private Dictionary<string, DomainEntity> _domainEntities;
        private List<string> _sequence;
        private ILogger _logger;
        public StartUpHandler(IDomainEntitySource domainSource, ILogger<StartUpHandler> logger)
        {
            this._logger = logger;
            this._domainSource = domainSource;
            this._domainEntities = this._domainSource.GetDomainEntities();
            this._sequence = this._domainSource.GetSequence();
        }
        public int StopAllDomainEntities(CancellationToken token)
        {
            int count = 0;
            foreach (string key in _sequence.AsEnumerable().Reverse())
            {
                if (!this._domainEntities.ContainsKey(key))
                { _logger.LogError($"{this.GetType().Name}._domainEnities do not contain Key: {key}"); }
                DomainEntity entity = this._domainEntities[key];
                if (entity.GetType() != typeof(WinService)) { continue; }
                var service = (WinService)entity;
                entity.Stop();
                var state = service.GetStatus();
                var winService = (WinService)entity;
                _logger.LogDebug($"{service.TechnicalName}: is in Status: [{state}]");
                count = state == System.ServiceProcess.ServiceControllerStatus.Stopped ? count + 1 : count;
            }
            return count;
        }
        public int StartAllDomainEntities(CancellationToken token)
        {
            //bool allStarted = false;
            int count = 0;
            foreach (string key in this._sequence)
            {
                count++;
                if (!this._domainEntities.ContainsKey(key))
                { throw new InvalidDataException($"{this.GetType().Name}._domainEnities do not contain Key: {key}"); }
                DomainEntity domainObj = this._domainEntities[key];
                bool domainStarted = TryStartingDomainEntity(domainObj, token);
                if (!domainStarted)
                {
                    string mssg = $"{domainObj.Name} could not be started. Shutting down Worker ...";
                    _logger.LogCritical(mssg);
                    throw new ApplicationException(mssg);
                }
            }
            return count;
        }

        public bool TryStartingDomainEntity(DomainEntity domainObj, CancellationToken cancellationToken)
        {
            bool domainObjStarted = false;
            ExecutionResult result;
            int retryCount = domainObj.MaxRetries;

            while (retryCount > 0 && !domainObjStarted)
            {
                if (cancellationToken.IsCancellationRequested) { break; }
                result = domainObj.Start();
                if (!result.IsSuccessfull)
                {
                    retryCount--;
                    _logger.LogDebug($"{domainObj.Name} start attempt failed: {result.Message} --- Retries left: {retryCount} ");
                    continue;
                }
                domainObjStarted = true;
                _logger.LogDebug($"{domainObj.Name} started successfull: {result.Message}.");
            }

            return domainObjStarted;

        }
    }
}
