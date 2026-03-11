using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStarter_v1.Main
{
    public class ExecutionResult
    {
        public bool IsSuccessfull { get; private set; }
        public string Message { get; private set; }
        public Exception? InnerException { get; private set; }

        public ExecutionResult(bool success,string mssg, Exception error)
        {
            IsSuccessfull = success;
            Message = mssg;
            InnerException = error;
        }
        public ExecutionResult(bool success, string mssg)
        {
            IsSuccessfull = success;
            Message = mssg;
        }


        override
        public string ToString()
        {
            return $"{{ " +
                $"\"IsSuccessfull\": {IsSuccessfull.ToString().ToLower()}," +
                $" \"Message\": \"{Message}\", " +
                $"\"InnerException\": \"{InnerException?.Message}\"" +
                $" }}";
        }
    }
}
