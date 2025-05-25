using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTos.Payment
{
    public class BalanceDto
    {
        [JsonPropertyName("totalBalance")]
        public decimal TotalBalance { get; set; }

        [JsonPropertyName("availableBalance")]
        public decimal AvailableBalance { get; set; }

        [JsonPropertyName("blockedBalance")]
        public decimal BlockedBalance { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("lastUpdated")]
        public string LastUpdated { get; set; }
    }
}
