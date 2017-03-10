using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CoreMicroService.Log
{
    //public interface ILog<T> where T:class
    //{
    //    void Log();
    //}
    public interface ILogger
    {
        void Log();
    }
}
