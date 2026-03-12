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
        //public ExecutionResult? InnerResult { get; private set; }

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

        public override string ToString()
        {
            string result = $"Class: {this.GetType().Name} || ";
            var props = GetType().GetProperties();

            var propStr = string.Join(", ",
                props
                    .Select(p => new { Prop = p, Value = p.GetValue(this) })
                    .Where(x => x.Value != null)
                    .Select(x => $"{x.Prop.Name}: {x.Value}")
            );

            return result + propStr;
        }
        
    }
}
