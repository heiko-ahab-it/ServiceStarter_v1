using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigLoader_v2
{
    internal class PortTestDTO: SequenceObjectDTO
    {
        public required String Server { get; set; }

        [Range(1, 65535)]
        public required int Port { get; set; } 
        public required PROTOCOL PROTOCOL { get; set; }

        [Range(100,599)]
        public required int startOk { get; set; }

        [Range(100,599)]
        public required int endOk { get; set; }

     }
}
