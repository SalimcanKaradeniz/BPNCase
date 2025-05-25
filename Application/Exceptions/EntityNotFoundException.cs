using Application.DTos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class EntityNotFoundException<T> : Exception
    {
        public BaseResponse<object> Response { get; }

        public EntityNotFoundException(string entityName, long? id = null)
            : base($"{entityName} not found")
        {
            Response = new BaseResponse<object>()
            {
                IsSuccess = false,
                ErrorMessage = $"{entityName} not found"
            };
        }
    }
}
