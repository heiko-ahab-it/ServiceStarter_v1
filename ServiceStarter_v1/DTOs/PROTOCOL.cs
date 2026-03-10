using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ConfigLoader_v2
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    internal enum PROTOCOL
    {
        HTTP, HTTPS
    }
}
