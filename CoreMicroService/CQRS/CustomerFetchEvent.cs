using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebAPI.CQRS
{
    public class CustomerFetchEvent : IEvent
    {
        public Guid CorrelationID { get; private set; }
        public string CustomerID { get; private set; }
        public string Name { get; private set; }
        public string UserId { get; set; }

        public CustomerFetchEvent(Guid correlationID,string customerID,string name,string userId)
        {
            //fetch customer attributes from cache
            CorrelationID = correlationID;
            CustomerID = customerID;
            Name = name;
            UserId = userId;
        }
    }
}
