using Application.DTos.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTos.Order
{
    public class CompleteOrderResponseDto
    {
        [JsonPropertyName("orderId")]
        public string OrderId { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("cancelledAt")]
        public string CancelledAt { get; set; }

        [JsonPropertyName("completedAt")]
        public string CompletedAt { get; set; }
    }

    public class CompleteOrderModel
    {
        [JsonPropertyName("order")]
        public CompleteOrderResponseDto Order { get; set; }

        [JsonPropertyName("updatedBalance")]
        public BalanceDto UpdatedBalance { get; set; }
    }
}
