using kit_stem_api.Models.DTO.Request;

namespace kit_stem_api.Services.IServices
{
    public interface IVNPayService
    {
        Task<ServiceResponse> CreatePaymentUrl(VNPaymentRequestDTO paymentRequest);
        Task<ServiceResponse> PaymentExecute(IQueryCollection vnPayData);
    }
}