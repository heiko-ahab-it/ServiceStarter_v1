using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigLoader_v2
{
    internal class MailServerDTO
    {
        public required String Host { get; set; }
        
        [Range(1, 65000)]
        public required int Port { get; set; } 
        public required string User { get; set; } 
        public required string Password { get; set; } 
    }
}
