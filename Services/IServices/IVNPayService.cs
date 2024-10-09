using kit_stem_api.Models.DTO.Request;

namespace kit_stem_api.Services.IServices
{
    public interface IVNPayService
    {
        Task<ServiceResponse> CreatePaymentUrl(PaymentVnPayCreateDTO paymentVnPayCreateDTO);
        Task<ServiceResponse> PaymentExecute(IQueryCollection vnPayData);
    }
}