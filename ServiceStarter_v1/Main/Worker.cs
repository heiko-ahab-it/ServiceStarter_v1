
using System.Security.Principal;
using ConfigLoader_v2;
using Microsoft.Extensions.Options;
using ServiceStarter_v1.DomainEntitys_MonitoredItems;
using ServiceStarter_v1.DTOs;

namespace ServiceStarter_v1.Main
{
    internal class Worker : MyBackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private StartUpHandler _startupHandler;
        private MonitoringHandler _monitoringHandler;
        private ConfigDTO _configDTO;
        private readonly GlobalConfigDTO _globalConfig;

        //Enviroment Variables
        private int MonitorDelay => _globalConfig.MonitorDelay *1000;

        public Worker(ILogger<Worker> logger, StartUpHandler startupHandler, MonitoringHandler monitoringHandler, IOptions<ConfigDTO> options)
        {
            _logger = logger;
            _startupHandler = startupHandler;
            _monitoringHandler = monitoringHandler;
            _configDTO = options.Value;
            _globalConfig = _configDTO.Config;
           
        }

        protected async override Task OnStartUp(CancellationToken token)
        {

            if (OperatingSystem.IsWindows())
            {
                if (! WIN_UserIsAdministrator()) { throw new ApplicationException($"The Worker must be started with Administrator Priviliges"); }
 
            }
            else if (!OperatingSystem.IsWindows())
            {
                _logger.LogCritical("Worker ist Currently only for Windows available");
                throw new PlatformNotSupportedException("This application only supports Windows.");
            } // Wenn Kein Windows -> Dienst beenden
           
            int count = this._startupHandler.StartAllDomainEntities(token);
            _logger.LogDebug($"Started {count} objects...");

            return;

        }
        #pragma warning disable CA1416
        private bool WIN_UserIsAdministrator()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }     
        }

        protected override async Task ExecutionCycle(CancellationToken token)
        {
            int count = this._monitoringHandler.ProcessAllMonitoredItems(token);
            _logger.LogDebug($"Processed {count} objects. --- waiting for next Loop for {MonitorDelay/1000} seconds.");
            await Task.Delay(MonitorDelay, token);
            return;
        }

        protected override Task OnShutdown(CancellationToken token)
        {
            Console.WriteLine("Shutting down..");
            Thread.Sleep(3000);
            Console.Read();
            return Task.CompletedTask;
        }
    }
}
