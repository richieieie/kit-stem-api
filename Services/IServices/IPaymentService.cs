using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kit_stem_api.Models.DTO.Request;

namespace kit_stem_api.Services.IServices
{
    public interface IPaymentService
    {
        Task<ServiceResponse> CreateCashAsync(PaymentCreateDTO paymentCreateDTO);
    }
}