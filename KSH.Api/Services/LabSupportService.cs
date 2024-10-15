using AutoMapper;
using KSH.Api.Models.Domain;
using KSH.Api.Models.DTO.Request;
using KSH.Api.Repositories;
using KSH.Api.Services;
using KSH.Api.Utils;
using KST.Api.Models.DTO.Request;
using KST.Api.Models.DTO.Response;
using KST.Api.Services.IServices;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Cryptography;

namespace KST.Api.Services
{
    public class LabSupportService : ILabSupportService
    {
        private readonly int sizePerPage = 20;
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public LabSupportService(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #region Service methods
        public async Task<ServiceResponse> GetAsync(LabSupportGetDTO getDTO)
        {
            try
            {
                var filter = GetFilter(getDTO);
                var (labSupports, totalPages) = await _unitOfWork.LabSupportRepository.GetFilterAsync(
                filter,
                null,
                skip: sizePerPage * getDTO.Page,
                take: sizePerPage
                );
                var labSupportsDTO = _mapper.Map<IEnumerable<LabSupportResponseDTO>>(labSupports);
                if (labSupports.Count() > 0)
                {
                    return new ServiceResponse()
                        .SetSucceeded(true)
                        .AddDetail("message", "Lấy danh sách LabSupoet thành công")
                        .AddDetail("data", new { totalPages, currentPage = (getDTO.Page + 1), labSupports = labSupportsDTO });
                }
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .SetStatusCode(StatusCodes.Status404NotFound)
                    .AddError("notFound", "Không tìm thấy danh sách Lab Support")
                    .AddDetail("message", "Lấy danh sách thành công")
                    .AddDetail("data", new { totalPages, currentPage = (getDTO.Page + 1), labSupports = labSupportsDTO });
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddError("outOfService", "Không thể lấy danh sách LabSupport lúc này")
                    .AddDetail("message", "Lấy danh sách thất bại");
            }
        }

        public async Task<ServiceResponse> CreateAsync(Guid orderId, Guid labId, int packageId)
        {
            try
            {
                var orderSupport = await _unitOfWork.OrderSupportRepository.GetFilterByIdAsync(orderId, packageId, labId);

                if (orderSupport == null)
                {
                    return new ServiceResponse()
                       .SetSucceeded(false)
                       .SetStatusCode(StatusCodes.Status404NotFound)
                       .AddDetail("message", "Gửi yêu cầu hổ trợ thất bại!")
                       .AddError("notFound", "Không tìm thấy bài lab cần hổ trợ!");
                }

                if (orderSupport.RemainSupportTimes == 0)
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .SetStatusCode(StatusCodes.Status400BadRequest)
                        .AddDetail("message", "Gửi yêu cầu hổ trợ thất bại!")
                        .AddError("invalidCredentials", "Bạn đã hết lượt hổ trợ!");
                }

                var allReadyLabSupport = await _unitOfWork.LabSupportRepository.GetByOrderSupportId(orderSupport.Id);
                if (allReadyLabSupport != null)
                {
                    if (!allReadyLabSupport.IsFinished)
                    {
                        return new ServiceResponse()
                            .SetSucceeded(false)
                            .SetStatusCode(StatusCodes.Status400BadRequest)
                            .AddDetail("message", "Gửi yêu cầu hổ trợ thất bại!")
                            .AddError("invalidCredentials", "Bạn đã gửi yêu cầu hổ trợ cho bài lab này!");
                    }
                }

                var id = Guid.NewGuid();

                var labSupport = new LabSupport()
                {
                    Id = id,
                    OrderSupportId = orderSupport.Id,
                    StaffId = null,
                    Rating = 0,
                    FeedBack = null,
                    IsFinished = false,
                    CreatedAt = TimeConverter.GetCurrentVietNamTime(),
                };

                await _unitOfWork.LabSupportRepository.CreateAsync(labSupport);
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Gửi yêu cầ hổ trợ thành công");
            }
            catch
            {
                return new ServiceResponse()
                       .SetSucceeded(false)
                       .SetStatusCode(StatusCodes.Status500InternalServerError)
                       .AddDetail("message", "Gửi yêu cầu hổ trợ thất bại!")
                       .AddError("outOfService", "Không thể gửi yêu cầu hổ trợ ngay lúc này!");
            }

        }
        public async Task<ServiceResponse> UpdateStaffAsync(String staffId, Guid labSupportId)
        {
            try
            {
                var labSupport = _unitOfWork.LabSupportRepository.GetById(labSupportId);
                if (labSupport == null)
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddError("notFound", "Không tìm thấy Lab Support này")
                        .AddDetail("message", "Không tìm thấy lab Support này");
                }
                labSupport.StaffId = staffId;
                if (await _unitOfWork.LabSupportRepository.UpdateAsync(labSupport))
                {
                    return new ServiceResponse()
                        .SetSucceeded(true)
                        .AddDetail("message", "Hỗ trợ khách hàng thành công");
                }
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddError("invalidCredentials", "Thông tin nhập không hợp lệ")
                    .AddDetail("message", "Hỗ trợ khách hàng thất bại");
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddError("outOfService", "Không thể hỗ trợ ngay lúc này")
                    .AddDetail("message", "Hỗ trợ khách hàng thất bại");
            }
        }
        public async Task<ServiceResponse> UpdateFinishedAsync(Guid labSupportId)
        {
            try
            {
                Expression<Func<LabSupport, bool>> filter = (l) => l.Id.Equals(labSupportId);
                var (labSupports, totalPages) = await _unitOfWork.LabSupportRepository.GetFilterAsync(
                    filter,
                    null,
                    null,
                    null,
                    query => query.Include(l => l.OrderSupport)
                    );
                var labSupport = labSupports.FirstOrDefault();
                if (labSupport == null)
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddError("notFound", "Không tìm thấy danh sách hỗ trợ")
                        .AddDetail("message", "Không tìm thấy lab support này");
                }
                else if (labSupport.StaffId == null || labSupport.StaffId == "")
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddError("invalidCredentials", "Thông tin không hợp lệ")
                        .AddDetail("message", "Cập nhật hỗ trợ thành công cho khách hàng thất bại");
                }
                labSupport.IsFinished = true;
                if (await _unitOfWork.LabSupportRepository.UpdateAsync(labSupport))
                {
                    var orderSupport = labSupport.OrderSupport;
                    orderSupport.RemainSupportTimes -= 1;
                    if (await _unitOfWork.OrderSupportRepository.UpdateAsync(orderSupport))
                    {
                        return new ServiceResponse()
                        .SetSucceeded(true)
                        .AddDetail("message", "Hỗ trợ thành công");
                    }
                    else
                    {
                        return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddError("outOfService", "Không thể cập nhật hỗ trợ ngay lúc này")
                        .AddDetail("message", "Cập nhật hỗ trợ thất bại");
                    }
                }
                else
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddError("outOfService", "Không thể cập nhật hỗ trợ ngay lúc này")
                        .AddDetail("message", "Cập nhật hỗ trợ thất bại");
                }
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddError("outOfService", "Không thể cập nhật hỗ trợ ngay lúc này")
                    .AddDetail("message", "Cập nhật hỗ trợ thất bại");
            }

        }
        public async Task<ServiceResponse> UpdateReviewAsync(LabSupportReviewUpdateDTO DTO)
        {
            try
            {
                var labSupport = await _unitOfWork.LabSupportRepository.GetByIdAsync(DTO.Id);
                if (labSupport == null)
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddError("notFound", "Không tìm thấy Lab Support này")
                        .AddDetail("message", "Cập nhật đánh giá thất bại");
                }
                else if (!labSupport.IsFinished)
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddError("Unavailable", "Không thể để lại đánh giá khi chưa hỗ trợ xong")
                        .AddDetail("message", "Cập nhật đánh giá thất bại");
                }
                labSupport.Rating = DTO.Rating;
                labSupport.FeedBack = DTO.FeedBack;
                if (await _unitOfWork.LabSupportRepository.UpdateAsync(labSupport))
                {
                    return new ServiceResponse()
                        .SetSucceeded(true)
                        .AddDetail("message", "Cập nhật đánh giá thành công");
                }
                else
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddError("invalidCredentials", "Thông tin nhập không hợp lệ")
                        .AddDetail("message", "Cập nhật đánh giá thất bại");
                }
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddError("outOfService", "Không thể hỗ trợ ngay lúc này")
                    .AddDetail("message", "Cập nhật đánh giá thất bại");
            }
        }
        public async Task<ServiceResponse> GetByCustomerId(String userId)
        {
            try
            {
                Expression<Func<LabSupport, bool>> filter = (l) => l.OrderSupport.Order.UserId.Equals(userId);
                var (labSupports, totalPages) = await _unitOfWork.LabSupportRepository.GetFilterAsync(
                    filter,
                    null,
                    null,
                    null,
                    query => query.Include(l => l.OrderSupport).ThenInclude(l => l.Order)
                    );
                if (labSupports == null)
                {
                    return new ServiceResponse()
                        .SetSucceeded(false);
                }
                var labSupportsDTO = _mapper.Map<IEnumerable<LabSupportResponseDTO>>(labSupports);
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("data", new { totalPages, labSupports = labSupportsDTO });
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddError("outOfService", "Không thể lấy Lab Support ngay lúc này")
                    .AddDetail("message", "Lấy danh sách lab Support thất bại");
            }
        }
        #endregion
        #region Methods that help service
        private Expression<Func<LabSupport, bool>> GetFilter(LabSupportGetDTO getDTO)
        {
            return (l) => l.IsFinished == getDTO.Supported;
        }
        #endregion
    }
}
