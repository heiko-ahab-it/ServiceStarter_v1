using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStarter_v1
{
    internal class ExecutionResult
    {
        public required Boolean IsSuccessfull {  get; set; }
        public required String Message { get; set; }
        public Exception? InnerException { get; set; }

        override
        public String ToString()
        {
            return $"{{ " +
                $"\"IsSuccessfull\": {IsSuccessfull.ToString().ToLower()}," +
                $" \"Message\": \"{Message}\", " +
                $"\"InnerException\": \"{InnerException?.Message}\"" +
                $" }}";
        }
    }
}
