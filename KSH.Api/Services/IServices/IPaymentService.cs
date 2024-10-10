using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KST.Api.Models.DTO.Request;

namespace KST.Api.Services.IServices
{
    public interface IPaymentService
    {
        Task<ServiceResponse> CreateCashAsync(PaymentCreateDTO paymentCreateDTO);
    }
}