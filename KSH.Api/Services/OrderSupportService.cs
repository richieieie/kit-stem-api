using KSH.Api.Repositories;
using KSH.Api.Services;
using KST.Api.Models.DTO.Request;
using KST.Api.Services.IServices;

namespace KST.Api.Services
{
    public class OrderSupportService : IOrderSupportService
    {
        private readonly int sizePerPage = 10;
        private readonly UnitOfWork _unitOfWork;
        public OrderSupportService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ServiceResponse> GetAsync(OrderSupportGetDTO getDTO)
        {
            try
            {
                var (OrderSupports, totalPages) = await _unitOfWork.OrderSupportRepository.GetFilterAsync(
                    null,
                    null,
                    skip: sizePerPage * getDTO.Page,
                    take: sizePerPage,
                    null
                    );
                if (OrderSupports.Count() > 0)
                {
                    return new ServiceResponse()
                        .SetSucceeded(true)
                        .AddDetail("message", "Lấy danh sách LabSupoet thành công")
                        .AddDetail("data", new { totalPages, curremtPage = (getDTO.Page + 1), labSupports = OrderSupports });
                }
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddError("invalidCredentials", "Thông tin không hợp lệ")
                    .AddDetail("message", "Lấy danh sách thất bại");
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddError("outOfService", "Không thể lấy danh sách LabSupport lúc này")
                    .AddDetail("message", "Lấy danh sách thất bại");
            }
        }
    }
}
