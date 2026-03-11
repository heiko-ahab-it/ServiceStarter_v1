using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStarter_v1.DomainEntitys_MonitoredItems;

namespace ServiceStarter_v1.Main
{
    internal class MonitoringHandler
    {
        private IMonitoredItemSource _source;
        private Dictionary<string,MonitoredItem> _items;
        private ILogger _logger;

        public MonitoringHandler(IMonitoredItemSource source, ILogger<MonitoringHandler> logger)
        {
            _source = source;
            _items = _source.GetMonitoredItems();
            _logger = logger;
        }

        public int ProcessAllMonitoredItems(CancellationToken token)
        {
            int processedItems = 0;
            foreach (string key in _items.Keys)
            {
                MonitoredItem item = _items[key];
                ExecutionResult result = item.ExecuteAsync(token);
                _logger.LogDebug($"Processed item: {item}:{item._domainEntity.Name} with result [{result.ToString()}]");
                processedItems++;
            }

            return processedItems;
        }
        
    }
}
