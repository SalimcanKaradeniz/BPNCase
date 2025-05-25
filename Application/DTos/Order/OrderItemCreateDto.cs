using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTos.Order
{
    public class OrderItemCreateDto
    {
        public string ProductCode { get; init; }
        public int Quantity { get; init; }
    }
}
