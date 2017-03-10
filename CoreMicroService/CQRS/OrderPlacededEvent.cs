using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebAPI.CQRS
{
    public class OrderPlacedEvent : IEvent
    {
        public Guid CorrelationID { get; private set; }
        public Guid OrderId { get; set; }
        public decimal Amount { get; private set; }
        public string UserId { get; set; }

        public OrderPlacedEvent(Guid correlationID,Guid orderId, string name,decimal amount,string userId)
        {
            CorrelationID = correlationID;
            OrderId = orderId;
            Amount = amount;
            UserId = userId;
        }
    }
}
