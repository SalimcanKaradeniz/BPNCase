using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTos.Payment
{
    public class PreOrderResponseDto
    {
        [JsonPropertyName("orderId")]
        public string OrderId { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }

    public class PreOrderResponse
    {
        [JsonPropertyName("PreOrder")]
        public PreOrderResponseDto PreOrder { get; set; }

        [JsonPropertyName("updatedBalance")]
        public BalanceDto UpdatedBalanceModel { get; set; }
    }
}
