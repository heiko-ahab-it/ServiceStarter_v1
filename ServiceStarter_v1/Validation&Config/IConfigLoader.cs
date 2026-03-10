using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStarter_v1
{
    internal interface IConfigLoader
    {
              
       
        T getRootObject<T>(string path) where T : class;
    }
}
