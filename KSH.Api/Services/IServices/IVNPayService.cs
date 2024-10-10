using KST.Api.Models.DTO.Request;

namespace KST.Api.Services.IServices
{
    public interface IVNPayService
    {
        Task<ServiceResponse> CreatePaymentUrl(PaymentVnPayCreateDTO paymentVnPayCreateDTO);
        Task<ServiceResponse> PaymentExecute(IQueryCollection vnPayData);
    }
}