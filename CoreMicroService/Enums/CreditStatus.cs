using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreMicroService.Enums
{
    public static class CreditStatus
    {
        public static readonly int DECLINED = 0;
        public static readonly int APPROVED = 1;
        public static readonly int PROCESSING = 2;
    }
}
