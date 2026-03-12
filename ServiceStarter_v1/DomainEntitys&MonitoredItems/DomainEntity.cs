using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStarter_v1.Main;

namespace ServiceStarter_v1.DomainEntitys_MonitoredItems
{
    internal abstract class DomainEntity
    {
        public string Name {get;private set;}
        public int MaxRetries { get;private set;}
        public int RecoveryTimeout {  get;private set;}

        public DomainEntity(string name,int maxRetries, int recoveryTimeout)
        {
            Name = name;
            MaxRetries = maxRetries;
            RecoveryTimeout = recoveryTimeout;
        }
        public abstract ExecutionResult StartAsync();
        public abstract ExecutionResult IsHealthy();
        public abstract ExecutionResult Recover();
        
        //public abstract Task<ExecutionResult> RecoverAsync();
        public abstract ExecutionResult Stop();

        public override string ToString()
        {
            string result = $" type: {this.GetType().Name}, ";
            var props = GetType().GetProperties();


            var propStr = string.Join(", ", props.Select(p => $"{p.Name}: {p.GetValue(this)}"));
            return result + propStr;
        }




    }
}
