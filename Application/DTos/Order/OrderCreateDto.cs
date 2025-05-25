using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTos.Order
{
    public class OrderCreateDto
    {
        public string Description { get; init; }
        public List<OrderItemCreateDto> Items { get; init; } = [];
    }
}
