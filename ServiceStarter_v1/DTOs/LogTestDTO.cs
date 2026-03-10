using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigLoader_v2
{
    internal class LogTestDTO: SequenceObjectDTO
    {
        public required String Path { get; set; } 
        public required String FileName { get; set; } 
        public required MODE Mode { get; set; } 
    }
}
