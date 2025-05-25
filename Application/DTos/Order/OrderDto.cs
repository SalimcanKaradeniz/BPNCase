using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTos.Order
{
    public class OrderDto
    {
        public long Id { get; set; }
        public int OrderStatusId { get; set; }
        public string OrderStatus { get; set; }
        public string UserId { get; set; }
        public decimal Total { get; set; }
        public List<OrderItemDto> Items { get; set; } = [];
    }
}
