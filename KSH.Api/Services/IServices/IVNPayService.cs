using KSH.Api.Models.DTO.Request;

namespace KSH.Api.Services.IServices
{
    public interface IVNPayService
    {
        Task<ServiceResponse> CreatePaymentUrl(PaymentVnPayCreateDTO paymentVnPayCreateDTO);
        Task<ServiceResponse> PaymentExecute(IQueryCollection vnPayData);
    }
}