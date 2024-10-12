using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KSH.Api.Models.DTO.Request;

namespace KSH.Api.Services.IServices
{
    public interface IPaymentService
    {
        Task<ServiceResponse> CreateCashAsync(PaymentCreateDTO paymentCreateDTO);
    }
}