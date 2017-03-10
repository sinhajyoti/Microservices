using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreMicroService.Models
{
    public class Order:BaseEntity
    {
        [Key]
        public Guid OrderId { get; set; }

        [Required]
        public string OrderName { get; set; }

        [Required]
        public decimal OrderAmount { get; set; }

        [Required]
        public int OrderStatus { get; set; }
        
        public string CorrelationId { get; set; }
        public int SequenceId { get; set; }
        public string LastUpdatedby { get; set; }
        public DateTime LastUpdatedOn { get; set; }
    }
}
