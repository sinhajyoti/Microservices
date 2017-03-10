using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreWebAPI.CQRS;

namespace CoreMicroService.Log
{
    public class OrderEventLogger
    {
        public IEvent evt { get; set; }
        public void log()
        {
            return;
        }
    }
}
