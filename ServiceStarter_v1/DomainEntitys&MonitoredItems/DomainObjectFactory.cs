using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigLoader_v2;
using Microsoft.Extensions.Options;
using ServiceStarter_v1.DTOs;

namespace ServiceStarter_v1.DomainEntitys_MonitoredItems
{
    internal class DomainObjectFactory: IDomainEntitySource,IMonitoredItemSource
    {
        private readonly ILogger<DomainObjectFactory> _logger;
        private readonly ConfigDTO _config;
        private readonly GlobalConfigDTO _globalConfig;
        private Dictionary<string, DomainEntity> _domainEntities;// List<DomainEntity> _domainEntities;
        private List<string> _sequence;
        private Type[] _typesForMonitoring = new Type[] {typeof(DUMMY), typeof(WinService),typeof(PortTest)};
        private Dictionary<string,MonitoredItem> _monitoredItems = new Dictionary<string,MonitoredItem>();
        private IServiceProvider _serviceProvider;
        
        public DomainObjectFactory(IOptions<ConfigDTO>options, ILogger<DomainObjectFactory> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _config = options.Value;
            _globalConfig = _config.Config;
            _domainEntities = new Dictionary<string, DomainEntity>();//new List<DomainEntity>();
            _sequence = new List<string>();
            _serviceProvider = serviceProvider;

            BuildDomainEntities();
            BuildSequence();
            BuildMonitoredItems();
        }
        public bool BuildSequence()
        {
            bool sequenceCompleteBuild = false;

            List<string> sequenceDTO = _config.Sequence;
            foreach(string item in sequenceDTO)
            {
                if (this._domainEntities.ContainsKey(item)) { this._sequence.Add(item); }
                else 
                { 
                    throw new InvalidDataException(
                        $"Object with Unique Name: {item} in Sequence, is not in Sequence Objects: {string.Join(",", this._domainEntities.Values)}"); 
                }
            }
            return sequenceCompleteBuild;
        }

      
        public bool BuildDomainEntities()
        {
            bool buildAllDomainEntitysSuccessfull = false;
            Dictionary<string, SequenceObjectDTO> sequenceObj = _config.SequenceObjects;
            _logger.LogTrace($"sequenceObject(DTO) count: {sequenceObj.Keys.Count}");
            foreach (var key in sequenceObj.Keys)
            {
                SequenceObjectDTO item = sequenceObj[key];
                               
                int maxRetry = item.MaxRetry;
                int recoveryTimeout = item.RecoveryTimeout;
                string uniqueName = key;

                using var loggerFactory = LoggerFactory.Create(builder =>
                {
                    
                    builder.AddConsole();               // Log-Ausgabe auf Konsole
                    builder.SetMinimumLevel(LogLevel.Trace);
                });

                ILogger<DUMMY> dummyLogger = loggerFactory.CreateLogger<DUMMY>();

                switch (item)
                {
                    case   PortTestDTO or LogTestDTO:



                        //var dummy;// = new DUMMY(uniqueName,maxRetry,recoveryTimeout,dummyLogger);
                        var dummy = ActivatorUtilities.CreateInstance<DUMMY>(_serviceProvider, uniqueName, maxRetry, recoveryTimeout);
                        this._domainEntities.Add(uniqueName, dummy);
                        break;

                    case ServiceDTO:
                       
                        string technicalName = ((ServiceDTO)item).ServiceName;
                        bool forceKill = this._globalConfig.ForceKillServices;
                  
                        if (!OperatingSystem.IsWindows())
                             throw new Exception(" Operating System is not Windows - software can currently only be used on Windows");
                        WinService service = ActivatorUtilities.CreateInstance<WinService>(_serviceProvider, uniqueName, maxRetry, recoveryTimeout,technicalName,forceKill);//new WinService(name, maxRetry, recoveryTimeout);
                        //this._domainEntities.Add(service);
                        this._domainEntities.Add(uniqueName,service);
                        break;
                /*
                    case PortTestDTO:
                        string server = ((PortTestDTO)item).Server;
                        int port = ((PortTestDTO)item).Port;
                        PROTOCOL protocol = ((PortTestDTO)item).PROTOCOL;
                        int startOk = ((PortTestDTO)item).startOk;
                        int endOk = ((PortTestDTO)item).endOk;
                        PortTest portTest = new PortTest(uniqueName,maxRetry,recoveryTimeout,server,port,protocol,startOk,endOk);
                        this._domainEntities.Add(uniqueName,portTest);
                        break;
                   
                    case LogTestDTO:
                        string fileName = ((LogTestDTO)item).FileName;
                        string path = ((LogTestDTO)item).Path;
                        LogTest logTest = new LogTest(uniqueName, maxRetry, recoveryTimeout, path, fileName);
                        this._domainEntities.Add(uniqueName,logTest);
                        break;*/
                    default:
                        throw new InvalidDataException($" Object in Config.SequenceObject{sequenceObj.GetType()} can not be of Type: {item.GetType()}");
                      
                }
             }
            foreach (var item in this._domainEntities) {this._logger.LogTrace(item.ToString()); }
            return buildAllDomainEntitysSuccessfull;
        }

        public Dictionary<string,DomainEntity> GetDomainEntities()
        {
            return this._domainEntities;
        }

        public List<string> GetSequence()
        {
            return this._sequence;
        }
        
        private void BuildMonitoredItems()
        {
            foreach (string key in this._domainEntities.Keys)
            {
                DomainEntity domainEntity = this._domainEntities[key];
                if (ShouldBeMonitored(domainEntity))
                {
                    MonitoredItem monItem = ActivatorUtilities.CreateInstance<MonitoredItem>(_serviceProvider, domainEntity);
                    monItem.RecoveryFinished += (result) =>
                    {
                        monItem._logger.LogDebug($"Recovery of {monItem._domainEntity.Name} finished! Result: {result.Message}");
                    };
                    this._monitoredItems.Add(key,monItem);
                }
            }  
        }
        private bool ShouldBeMonitored<T>(T domainObject) where T : notnull
        {
            return this._typesForMonitoring.Contains(domainObject.GetType());
        }


        public Dictionary<string, MonitoredItem> GetMonitoredItems()
        {
            return this._monitoredItems;
        }
    }
}
