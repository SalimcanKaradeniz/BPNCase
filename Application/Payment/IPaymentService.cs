using Application.Payment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Payment
{
    public interface IPaymentService
    {
        Task<decimal> GetAvailableBalanceAsync();
        Task<ProcessPaymentModel> StartPaymentAsync(ProcessPaymentCreateModel processPaymentCreateModel);
        Task<bool> CompletePaymentAsync(long orderId);
    }
}
