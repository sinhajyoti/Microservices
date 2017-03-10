using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebAPI.CQRS
{
    public class OrderDeletedEvent : IEvent
    {
        public Guid CorrelationID { get; private set; }
        public Guid OrderId { get; private set; }
        public string UserId { get; set; }
        public OrderDeletedEvent(Guid correlationID,Guid orderId,string userId)
        {
            CorrelationID = correlationID;
            OrderId = orderId;
            UserId = userId;
        }
    }
}
