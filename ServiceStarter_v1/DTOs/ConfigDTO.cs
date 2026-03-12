using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ServiceStarter_v1.DTOs;

namespace ConfigLoader_v2
{
    internal class ConfigDTO
    {
        [JsonPropertyName("config")] 
        [Required]
        public required GlobalConfigDTO Config { get; set; }
        //public required Dictionary<string, object> Config { get; set; }
        
        [JsonPropertyName("sequence")]
        [Required]
        public required List<String> Sequence { get; set; }

        [JsonPropertyName("sequenceObjects")]
        [Required]
        public required Dictionary<string, SequenceObjectDTO> SequenceObjects { get; set; }

        [JsonPropertyName("mailServer")]
        [Required]
        public required Dictionary<string, MailServerDTO> MailServer { get; set; } 
    }
}
