using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using ServiceStarter_v1.Main;

namespace ServiceStarter_v1.DomainEntitys_MonitoredItems
{
    internal class WinService : DomainEntity
    {
        private ILogger _logger;
        private ServiceController _service;
        [Description("If Stopping des Service fails, the ForceKill will kill the Process of the Service")]
        private Boolean ForceKillProcess { get; set; }
        private String TechnicalName { get; set; }

        [Description("Max Time in seconds the Controller waits for the Service to reach the State 'Running' after trying to Start the Service.")]
        private int MaxBootUpTime { get; set; }
        public WinService(string name, int maxRetries, int restartTimeout, string technicalName, ILogger<WinService> logger) : base(name, maxRetries, restartTimeout)
        {
            _logger = logger;
            if (!OperatingSystem.IsWindows()) { throw new PlatformNotSupportedException($"{this.GetType().Name} can only by instantiated on Windows-Devices"); }
            TechnicalName = technicalName;
            MaxBootUpTime = 30;
            ForceKillProcess = false;
            try
            {
                _service = new ServiceController(TechnicalName);

            }catch (ArgumentException) { throw; }

            

        }

        #pragma warning disable CA1416
        public override ExecutionResult IsHealthy()
        {
            this._service.Refresh();
            bool isHealthy = this._service.Status == ServiceControllerStatus.Running ? true : false;
            return new ExecutionResult(isHealthy, $"{this.TechnicalName} : {this._service.Status}");
        }

        
        public override ExecutionResult Recover()
        {
            return this.Start();
        }

        public override ExecutionResult Start()
        {
            this._service.Refresh();
            if (this._service.Status == ServiceControllerStatus.Stopped) {
                try
                {
                    this._service.Start();
                    this._service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(MaxBootUpTime));
                } catch (System.ServiceProcess.TimeoutException)
                { return new ExecutionResult(false, $"{this.TechnicalName} got stuck during the Starting Process, stopped Boot up after {this.MaxBootUpTime} Seconds"); }
                catch (Exception err) {
                    this._logger.LogError($"{this.TechnicalName} failed Boot Up. \n {err.Message} \n {err.StackTrace} \n {err.InnerException} \n {err.Source}\n");
                    return new ExecutionResult(false, $"Boot up for {this.TechnicalName} failed, due to {err.Message}\n {err.StackTrace}"); }
            } 
            return this.IsHealthy();
        }

        public override ExecutionResult Stop()
        {
            this._service.Refresh();
            if (this._service.Status != ServiceControllerStatus.Stopped)
            {
                try
                {
                    this._service.Stop();
                    this._service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(MaxBootUpTime * 3));
                }
                catch (Exception err)
                {
                    this._logger.LogError($"{this.TechnicalName} failed to Stop. \n {err.Message} \n {err.StackTrace} \n {err.InnerException} \n {err.Source}\n");
                    return new ExecutionResult(false, $"Stopping {this.TechnicalName} failed, due to [{err.Message}]");
                }
            }
            return this.IsHealthy();
        }
            
    }
}
