using AutoMapper;
using KST.Api.Models.Domain;
using KST.Api.Models.DTO.Request;
using KST.Api.Models.DTO.Response;
using KST.Api.Repositories;
using KST.Api.Services.IServices;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
        public async Task<ServiceResponse> GetAsync(LabSupportGetDTO getDTO)
        {
            try
            {
                var (labSupports, totalPages) = await _unitOfWork.LabSupportRepository.GetFilterAsync(
                null,
                null,
                skip: sizePerPage * getDTO.Page,
                take: sizePerPage,
                query => query.Include(l => l.Staff).Include(l => l.OrderSupport)
                );
                var labSupportsDTO = _mapper.Map<IEnumerable<LabSupportResponseDTO>>(labSupports);
                if (labSupports.Count() > 0)
                {
                    return new ServiceResponse()
                        .SetSucceeded(true)
                        .AddDetail("message", "Lấy danh sách LabSupoet thành công")
                        .AddDetail("data", new { totalPages, curremtPage = (getDTO.Page + 1), labSupports = labSupportsDTO });
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

        public async Task<ServiceResponse> CreateAsync(Guid orderId)
        {
            try
            {
                var labSupport = new LabSupport()
                {
                    Id = Guid.NewGuid(),
                    OrderSupportId = orderId,
                    StaffId = null,
                    Rating = 0,
                    FeedBack = null,
                    IsFinished = false,
                    CreatedAt = DateTimeOffset.Now,
                };
                if (await _unitOfWork.LabSupportRepository.CreateAsync(labSupport))
                {
                    return new ServiceResponse()
                        .SetSucceeded(true)
                        .AddDetail("message", "Tạo yêu cầu hỗ trợ thành công");
                }
                else
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddError("invalidCredentials", "Thông tin gửi không hợp lệ")
                        .AddDetail("message", "Tạo yêu cầu thất bại");
                }
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddError("outOfService", "Không thể tạo yêu cầu lúc này")
                    .AddDetail("message", "Tạo yêu cầu thất bại");
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
                    .AddDetail("message", "Hỗ trợ khách hàng thành công");
            }
        }

        
    }
}
