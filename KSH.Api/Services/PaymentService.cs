using KST.Api.Constants;
using KST.Api.Models.Domain;
using KST.Api.Models.DTO.Request;
using KST.Api.Repositories;
using KST.Api.Services.IServices;

namespace KST.Api.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly UnitOfWork _unitOfWork;
        public PaymentService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ServiceResponse> CreateCashAsync(PaymentCreateDTO paymentCreateDTO)
        {
            var serviceResponse = new ServiceResponse();
            try
            {
                var order = await _unitOfWork.OrderRepository.GetByIdAsync(paymentCreateDTO.OrderId);
                if (order == null)
                {
                    return serviceResponse
                            .SetSucceeded(false)
                            .SetStatusCode(StatusCodes.Status404NotFound)
                            .AddDetail("message", "Tạo mới payment thất bại!")
                            .AddError("notFound", "Không tìm thấy order của bạn!");
                }

                var payment = new Payment()
                {
                    Id = Guid.NewGuid(),
                    MethodId = OrderFulfillmentConstants.PaymentCash,
                    CreatedAt = DateTimeOffset.Now,
                    Status = OrderFulfillmentConstants.PaymentFail,
                    Amount = order.TotalPrice,
                    OrderId = order.Id
                };
                order.ShippingStatus = OrderFulfillmentConstants.OrderVerifyingStatus;

                await _unitOfWork.PaymentRepository.CreateAsync(payment);
                await _unitOfWork.OrderRepository.UpdateAsync(order);

                return serviceResponse
                        .AddDetail("message", "Giao dịch thành công!");
            }
            catch
            {
                return serviceResponse
                        .SetSucceeded(false)
                        .AddDetail("message", "Tạo mới payment thất bại!")
                        .AddError("outOfService", "Không thể tạo một order mới ngay lúc này!");
            }

        }
    }
}