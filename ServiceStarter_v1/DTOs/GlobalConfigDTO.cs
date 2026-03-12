using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ServiceStarter_v1.DTOs
{
    internal class GlobalConfigDTO
    {
        [JsonPropertyName("WIN_OS")]
        public required bool WinOs { get; set; }


        [JsonPropertyName("LOG_PATH")]
        public required string LogPath { get; set; }


        [JsonPropertyName("MONITOR_DELAY")]
        public int MonitorDelay { get; set; } = 30;


    }
}
