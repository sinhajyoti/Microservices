using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebAPI.CQRS
{
    public class CheckCreditCommand:IEvent
    {
        public Guid CorrelationID { get; private set; }
        public Guid OrderId { get; set; }
        public decimal OrderAmount { get; private set; }
        public int CreditStatus { get; private set; }//refer enum->CreditStatus

        public string UserId { get; set; }
        public CheckCreditCommand(Guid correlationID,Guid orderId, decimal orderamount,string userId)
        {
            var creditStatus = 1;//get credit check from DB/Service bus
            CorrelationID = correlationID;
            OrderId = orderId;
            OrderAmount = orderamount;
            CreditStatus = creditStatus;
        }
    }
}
