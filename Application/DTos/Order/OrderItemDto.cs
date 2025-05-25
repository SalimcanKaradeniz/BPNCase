using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTos.Order
{
    public record OrderItemDto
    {
        public long Id { get; set; }
        public string ProductCode { get; set; }
        public string UserId { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
    }
}
