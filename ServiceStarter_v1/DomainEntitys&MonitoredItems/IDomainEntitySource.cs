using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStarter_v1.DomainEntitys_MonitoredItems
{
    internal interface IDomainEntitySource
    {
        public Dictionary<string,DomainEntity> GetDomainEntities();
        public List<string> GetSequence();
    }
}
