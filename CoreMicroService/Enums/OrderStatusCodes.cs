using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreMicroService.Enums
{
    public static class OrderStatusCodes
    {
        public static readonly int ORDER_CREATED = 0;
        public static readonly int ORDER_CHECKEDOUT = 1;
        public static readonly int ORDER_PLACED = 2;
        public static readonly int ORDER_PMTDECLINED = 3;
        public static readonly int ORDER_CANCELLED = 4;
        public static readonly int ORDER_CANCELLED_BY_VENDOR = 5;
        public static readonly int ORDER_SHIPPED = 6;
        public static readonly int ORDER_CLOSED = 7;
        public static readonly int ORDER_REFUNDED = 8;
        public static readonly int ORDER_REFUNDED_PARTIALLY = 9;
        public static readonly int ORDER_DELETED = 10;
    }
}
