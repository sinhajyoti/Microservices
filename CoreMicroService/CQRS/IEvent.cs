using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebAPI.CQRS
{
    public interface IEvent
    {
        Guid CorrelationID { get; }
        string UserId { get; } 
    }
}
