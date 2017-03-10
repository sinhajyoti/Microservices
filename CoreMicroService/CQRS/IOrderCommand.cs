using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebAPI.CQRS
{
    public interface IOrderCommand
    {
        Guid CorrelationID { get; set; }
    }
}
