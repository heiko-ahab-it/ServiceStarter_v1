using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ConfigLoader_v2
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "category")]
    [JsonDerivedType(typeof(ServiceDTO), typeDiscriminator: "service")]
    [JsonDerivedType(typeof(PortTestDTO), typeDiscriminator: "port_test")]
    [JsonDerivedType(typeof(LogTestDTO), typeDiscriminator: "log_test")]
    public class SequenceObjectDTO
    {
        [JsonPropertyName("maxRetry")]
        [JsonRequired]
        [Range(0,int.MaxValue)]
        public int MaxRetry { get; set; }

        [JsonRequired]
        [JsonPropertyName("recoveryTimeout")]
        [Range(5,int.MaxValue)] // at least 5 seconds musst be between service restarts
        public int RecoveryTimeout { get; set; }
        
        public override string ToString()
        {
            string result = $" type: {this.GetType().Name}, ";
            var props = GetType().GetProperties();

            
            var propStr = string.Join(", ",props.Select(p => $"{p.Name}: {p.GetValue(this)}"));
            return result + propStr ;
        }
    }
}
  