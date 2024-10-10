using KST.Api.Models.Domain;
using KST.Api.Models.DTO;
using KST.Api.Services.IServices;
using KST.Api.Repositories;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using KST.Api.Models.DTO.Response;
using KST.Api.Constants;

namespace KST.Api.Services
{
    public class LabService : ILabService
    {
        private readonly int sizePerPage = 20;
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public LabService(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #region Service methods
        public async Task<ServiceResponse> CreateAsync(LabUploadDTO labUploadDTO, Guid id, string url)
        {
            try
            {
                var lab = _mapper.Map<Lab>(labUploadDTO);
                lab.Id = id;
                lab.Url = url;
                await _unitOfWork.LabRepository.CreateAsync(lab);

                return new ServiceResponse()
                        .SetSucceeded(true)
                        .AddDetail("message", "Thêm mới bài lab thành công!");
            }
            catch
            {
                return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Thêm mới bài lab thất bại!")
                        .AddError("outOfService", "Không thể tạo mới bài lab ngay lúc này");
            }
        }
        public async Task<ServiceResponse> UpdateAsync(LabUpdateDTO labUpdateDTO, string? url)
        {
            try
            {
                var lab = await _unitOfWork.LabRepository.GetByIdAsync(labUpdateDTO.Id);
                if (lab == null)
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .SetStatusCode(StatusCodes.Status404NotFound)
                        .AddDetail("message", "Chỉnh sửa bài lab thất bại!")
                        .AddError("invalidCredentials", "Không tìm thấy bài lab để chỉnh sửa!");
                }

                lab.LevelId = labUpdateDTO.LevelId;
                lab.KitId = labUpdateDTO.KitId;
                lab.Name = labUpdateDTO.Name!;
                lab.Author = labUpdateDTO.Author;
                lab.Price = labUpdateDTO.Price;
                lab.MaxSupportTimes = labUpdateDTO.MaxSupportTimes;
                if (url != null)
                {
                    lab.Url = url;
                }

                await _unitOfWork.LabRepository.UpdateAsync(lab);

                return new ServiceResponse()
                        .SetSucceeded(true)
                        .AddDetail("message", "Chỉnh sửa bài lab thành công!");
            }
            catch
            {
                return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Chỉnh sửa bài lab thất bại!")
                        .AddError("outOfService", "Không thể tạo mới bài lab ngay lúc này");
            }
        }
        public async Task<ServiceResponse> GetAsync(LabGetDTO labGetDTO)
        {
            try
            {
                Expression<Func<Lab, bool>> filter = GetFilter(labGetDTO);

                var (labs, totalPages) = await _unitOfWork.LabRepository.GetFilterAsync(filter, null, skip: sizePerPage * labGetDTO.Page, take: sizePerPage);

                var labDTOs = _mapper.Map<IEnumerable<LabResponseDTO>>(labs);
                return new ServiceResponse()
                            .AddDetail("message", "Lấy thông tin các bài lab thành công!")
                            .AddDetail("data", new { totalPages, currentPage = labGetDTO.Page, labs = labDTOs });
            }
            catch
            {
                return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Lấy thông tin các bài lab thành công!")
                        .AddError("outOfService", "Không thể lấy được thông tin các bài lab hiện tại hoặc vui lòng kiểm tra lại thông tin!");
            }
        }
        public async Task<ServiceResponse> GetByIdAsync(Guid id)
        {
            try
            {
                var lab = await _unitOfWork.LabRepository.GetByIdAsync(id);
                var labDTO = _mapper.Map<LabResponseDTO>(lab);
                return new ServiceResponse()
                            .AddDetail("message", "Lấy thông tin bài lab thành công!")
                            .AddDetail("data", new { lab });
            }
            catch
            {
                return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Lấy thông tin bài lab không thành công!")
                        .AddError("outOfService", "Không thể lấy được thông tin bài lab hiện tại hoặc vui lòng kiểm tra lại thông tin!");
            }
        }
        public async Task<ServiceResponse> GetByKitId(int kitId)
        {
            try
            {
                var (labs, totalPages) = await _unitOfWork.LabRepository.GetByKitIdAsync(kitId);
                var labDTOs = _mapper.Map<IEnumerable<LabInPackageResponseDTO>>(labs);
                return new ServiceResponse()
                            .AddDetail("message", "Lấy thông tin bài lab thành công!")
                            .AddDetail("data", new { totalPages, currentPage = 0, labs = labDTOs });
            }
            catch
            {
                return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Lấy thông tin bài lab không thành công!")
                        .AddError("outOfService", "Không thể lấy được thông tin bài lab hiện tại hoặc vui lòng kiểm tra lại thông tin!");
            }
        }

        public async Task<ServiceResponse> RemoveByIdAsync(Guid id)
        {
            try
            {
                var lab = await _unitOfWork.LabRepository.GetByIdAsync(id);
                if (lab == null)
                {
                    return new ServiceResponse()
                                .SetSucceeded(false)
                                .SetStatusCode(StatusCodes.Status404NotFound)
                                .AddDetail("message", "Xoá bài lab thất bại")
                                .AddError("notFound", "Không tìm thấy bài lab của bạn");
                }

                lab.Status = false;
                await _unitOfWork.LabRepository.UpdateAsync(lab);

                return new ServiceResponse()
                            .AddDetail("message", "Xoá bài lab thành công!");
            }
            catch
            {
                return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Xoá bài lab không thành công!")
                        .AddError("outOfService", "Không thể xoá được bài lab hiện tại!");
            }
        }

        public async Task<ServiceResponse> RestoreByIdAsync(Guid id)
        {
            try
            {
                var lab = await _unitOfWork.LabRepository.GetByIdAsync(id);
                if (lab == null)
                {
                    return new ServiceResponse()
                                .SetSucceeded(false)
                                .SetStatusCode(StatusCodes.Status404NotFound)
                                .AddDetail("message", "Khôi phục bài lab thất bại!")
                                .AddError("notFound", "Không tìm thấy bài lab của bạn");
                }

                lab.Status = true;
                await _unitOfWork.LabRepository.UpdateAsync(lab);

                return new ServiceResponse()
                            .AddDetail("message", "Khôi phục bài lab thành công!");
            }
            catch
            {
                return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Khôi phục bài lab thất bại!")
                        .AddError("outOfService", "Không thể khôi phục được bài lab hiện tại!");
            }
        }
        public async Task<ServiceResponse> GetFileUrlByIdAsync(Guid id)
        {
            var serviceResponse = new ServiceResponse();
            try
            {
                var lab = await _unitOfWork.LabRepository.GetByIdAsync(id);
                if (lab == null)
                {
                    return serviceResponse
                        .SetSucceeded(false)
                        .SetStatusCode(StatusCodes.Status404NotFound)
                        .AddDetail("message", "Lấy thông tin bài lab không thành công!")
                        .AddError("notFound", "Không tìm thấy bài lab!");
                }

                return serviceResponse
                            .AddDetail("url", lab.Url)
                            .AddDetail("fileName", lab.Name);
            }
            catch
            {
                return serviceResponse
                        .SetSucceeded(false)
                        .AddDetail("message", "Lấy thông tin bài lab không thành công!")
                        .AddError("outOfService", "Không thể lấy được thông tin bài lab hiện tại hoặc vui lòng kiểm tra lại thông tin!");
            }
        }

        public async Task<ServiceResponse> GetFileUrlByIdAndOrderIdAsync(string userId, Guid labId, Guid orderId)
        {
            var serviceResponse = new ServiceResponse();
            try
            {
                var orderSupport = await _unitOfWork.OrderSupportRepository.GetByLabIdAndOrderIdAsync(labId, orderId);
                var payment = await _unitOfWork.PaymentRepository.GetByOrderId(orderId);
                if (orderSupport == null || orderSupport.Order.UserId != userId || payment == null)
                {
                    return serviceResponse
                        .SetSucceeded(false)
                        .SetStatusCode(StatusCodes.Status404NotFound)
                        .AddDetail("message", "Lấy thông tin bài lab không thành công!")
                        .AddError("notFound", "Bạn chưa mua sản phẩm nào bao gồm bài lab này!");
                }

                if (orderSupport.Order.ShippingStatus != OrderFulfillmentConstants.OrderSuccessStatus ||
                payment.Status != OrderFulfillmentConstants.PaymentSuccess)
                {
                    return serviceResponse
                        .SetSucceeded(false)
                        .SetStatusCode(StatusCodes.Status500InternalServerError)
                        .AddDetail("message", "Lấy thông tin bài lab không thành công!")
                        .AddError("unavailable", "Bạn vui lòng thanh toán và chờ đợi đơn hàng giao tới mình để có thể tải được bài lab!");
                }

                var lab = orderSupport.Lab;
                return serviceResponse
                            .AddDetail("url", lab.Url)
                            .AddDetail("fileName", lab.Name);
            }
            catch
            {
                return serviceResponse
                        .SetSucceeded(false)
                        .AddDetail("message", "Lấy thông tin bài lab không thành công!")
                        .AddError("outOfService", "Không thể lấy được thông tin bài lab hiện tại hoặc vui lòng kiểm tra lại thông tin!");
            }
        }
        #endregion

        #region Methods that are constructing arguments for PackageRepository
        private Expression<Func<Lab, bool>> GetFilter(LabGetDTO labGetDTO)
        {
            return (l) => l.Kit.Name.Contains(labGetDTO.KitName ?? "") &&
                        l.Name.Contains(labGetDTO.LabName ?? "");
        }
        #endregion
    }
}