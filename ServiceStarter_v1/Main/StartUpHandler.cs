using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using ServiceStarter_v1.DomainEntitys_MonitoredItems;

namespace ServiceStarter_v1.Main
{
    internal class StartUpHandler
    {
        private IDomainEntitySource _domainSource;
        private Dictionary<string,DomainEntity> _domainEntities;
        private List<string> _sequence;
        public StartUpHandler(IOptions<IDomainEntitySource> domainSource)
        {
            this._domainSource = domainSource.Value;
            this._domainEntities = this._domainSource.GetDomainEntities();
            this._sequence = this._domainSource.GetSequence();
        }
        public bool StartAllDomainEntities()
        {
            bool allStarted = false;
            return allStarted;
        }
    }
}
