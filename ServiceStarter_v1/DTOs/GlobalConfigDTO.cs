using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ServiceStarter_v1.DTOs
{
    [Description("Defines Default Values for Enviroment Variables.")]
    internal class GlobalConfigDTO
    {
        [JsonPropertyName("WIN_OS")]
        public required bool WinOs { get; set; }


        [JsonPropertyName("LOG_PATH")]
        public required string LogPath { get; set; }


        [JsonPropertyName("MONITOR_DELAY")]
        public int MonitorDelay { get; set; } = 30;

       /* [JsonPropertyName("FORCE_KILL_SERVICES")]

        public bool ForceKillServices { get; set; } = false;*/


    }
}
