using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ConfigLoader_v2
{
    public class ServiceDTO: SequenceObjectDTO
    {
        public required String ServiceName { get;set; }
       
        
        [JsonPropertyName("delay")]
        public required int Delay { get; set; }

 
    }
}
