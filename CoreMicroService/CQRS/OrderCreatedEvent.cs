using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebAPI.CQRS
{
    public class OrderCreatedEvent : IEvent
    {
        public Guid CorrelationID { get; private set; }
        public Guid OrderID { get; set; }
        public string Name { get; private set; }
        public decimal Amount { get; private set; }
        public string UserId { get; private set; }

        public OrderCreatedEvent(Guid correlationID,Guid orderId, string name,decimal amount,string userId)
        {
            CorrelationID = correlationID;
            OrderID = orderId;
            Name = name;
            Amount = amount;
            UserId = userId;
        }
    }
}
