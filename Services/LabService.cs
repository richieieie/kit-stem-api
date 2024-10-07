using kit_stem_api.Models.Domain;
using kit_stem_api.Models.DTO;
using kit_stem_api.Services.IServices;
using kit_stem_api.Repositories;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using kit_stem_api.Models.DTO.Response;

namespace kit_stem_api.Services
{
    public class LabService : ILabService
    {
        private readonly int sizePerPage = 5;
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
                // Construct file
                Expression<Func<Lab, bool>> filter = GetFilter(labGetDTO);

                // Try to get an IEnumerable<Lab> and total pages 
                var (labs, totalPages) = await _unitOfWork.LabRepository.GetFilterAsync(filter, null, skip: sizePerPage * labGetDTO.Page, take: sizePerPage);

                // Map IEnumerable<Lab> to IEnumerable<LabResponseDTO> using AutoMapper
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