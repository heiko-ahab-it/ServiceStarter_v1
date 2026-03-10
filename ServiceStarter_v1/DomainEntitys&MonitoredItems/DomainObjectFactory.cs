using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigLoader_v2;
using Microsoft.Extensions.Options;

namespace ServiceStarter_v1.DomainEntitys_MonitoredItems
{
    internal class DomainObjectFactory
    {
        private readonly ILogger<DomainObjectFactory> _logger;
        private readonly ConfigDTO _config;
        private List<DomainEntity> _domainEntities;
        
        public DomainObjectFactory(IOptions<ConfigDTO>options, ILogger<DomainObjectFactory> logger)
        {
            _logger = logger;
            _config = options.Value;
            _domainEntities = new List<DomainEntity>();

            Build();
        }
        public bool Build()
        {
            bool buildAllDomainEntitysSuccessfull = false;
            Dictionary<string, SequenceObjectDTO> sequenceObj = _config.SequenceObjects;
            foreach (var key in sequenceObj.Keys)
            {
                SequenceObjectDTO item = sequenceObj[key];
                               
                int maxRetry = item.MaxRetry;
                int recoveryTimeout = item.RecoveryTimeout;
                string uniqueName = key;
                switch (item)
                {
                    case ServiceDTO:
                       
                        string name = ((ServiceDTO)item).ServiceName;
                  
                        if (!OperatingSystem.IsWindows())
                             throw new Exception(" Operating System is not Windows - software can currently only be used on Windows");
                        WinService service = new WinService(name, maxRetry, recoveryTimeout);
                        this._domainEntities.Add(service);
                        break;
                    case PortTestDTO:
                        string server = ((PortTestDTO)item).Server;
                        int port = ((PortTestDTO)item).Port;
                        PROTOCOL protocol = ((PortTestDTO)item).PROTOCOL;
                        int startOk = ((PortTestDTO)item).startOk;
                        int endOk = ((PortTestDTO)item).endOk;
                        PortTest portTest = new PortTest(uniqueName,maxRetry,recoveryTimeout,server,port,protocol,startOk,endOk);
                        this._domainEntities.Add(portTest);
                        break;
                   
                    case LogTestDTO:
                        string fileName = ((LogTestDTO)item).FileName;
                        string path = ((LogTestDTO)item).Path;
                        LogTest logTest = new LogTest(uniqueName, maxRetry, recoveryTimeout, path, fileName);
                        this._domainEntities.Add(logTest);
                        break;
                    default:
                        throw new InvalidDataException($" Object in Config.SequenceObject{sequenceObj.GetType()} can not be of Type: {item.GetType()}");
                      
                }
             }
            this._domainEntities.ForEach(item => this._logger.LogTrace(item.ToString()));
   
           
            return buildAllDomainEntitysSuccessfull;
        }
    }
}
