using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreMicroService.Repository;
using CoreMicroService.Models;
using CoreMicroService.Enums;

namespace CoreWebAPI.CQRS
{
    public class OrderCommand:IOrderCommand
    {
        private OrderRepository orderRepo;
        public Guid CorrelationID { get; set; }

        void Apply(OrderCreatedEvent orderCreatedEvent)
        {
            CorrelationID = orderCreatedEvent.CorrelationID;
            //1. Update cache

            //2. save in DB
            orderRepo.Add(new Order { OrderId = orderCreatedEvent.OrderID, OrderName = orderCreatedEvent.Name, OrderAmount = orderCreatedEvent.Amount, OrderStatus = OrderStatusCodes.ORDER_CREATED, CorrelationId = orderCreatedEvent.CorrelationID.ToString(), LastUpdatedby = orderCreatedEvent.UserId });
            //3. publish orderCreatedEvent on Service Bus
        }

        void Apply(OrderPlacedEvent orderPlacedEvent)
        {
            //1. Update cache

            //2. save in DB
            orderRepo.Update(new Order { OrderId = orderPlacedEvent.OrderId, OrderAmount = orderPlacedEvent.Amount, OrderStatus = OrderStatusCodes.ORDER_PLACED, CorrelationId = orderPlacedEvent.CorrelationID.ToString(), LastUpdatedby = orderPlacedEvent.UserId });
            //3. publish orderPlacedEvent on Service Bus
        }

        void Apply(OrderDeletedEvent orderDeletedEvent)
        {
            //1. Update cache
            
            //2. save in DB
            orderRepo.Delete(new Order { OrderId = orderDeletedEvent.OrderId, OrderStatus = OrderStatusCodes.ORDER_DELETED, CorrelationId = orderDeletedEvent.CorrelationID.ToString(), LastUpdatedby = orderDeletedEvent.UserId });
            //3. publish orderPlacedEvent on Service Bus

        }
    }
}
