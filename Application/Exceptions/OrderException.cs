using Application.DTos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public interface IOrderException
    {
        BaseResponse<object> Response { get; }
    }

    public class OrderException<T> : Exception, IOrderException
    {
        public BaseResponse<object> Response { get; }

        public OrderException(string message, string? orderId = null)
    : base(!string.IsNullOrEmpty(orderId) ? $"{message}, SiparişNo: {orderId}" : message)
        {
            var errorMessage = string.Empty;
            if (!string.IsNullOrEmpty(orderId))
            {
                errorMessage = $"{orderId} numaralı sipariş bulunamadı";
            }

            Response = new BaseResponse<object>()
            {
                IsSuccess = false,
                ErrorMessage = $"{message},/// {errorMessage}."
            };
        }
    }


}
