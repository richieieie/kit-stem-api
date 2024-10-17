using KSH.Api.Models.DTO.Request;
using KSH.Api.Models.DTO.Response;

namespace KSH.Api.Services.IServices
{
    public interface IVNPayService
    {
        Task<ServiceResponse> CreatePaymentUrl(PaymentVnPayCreateDTO paymentVnPayCreateDTO);
        Task<(ServiceResponse, OrderResponseDTO)> PaymentExecute(IQueryCollection vnPayData);
    }
}