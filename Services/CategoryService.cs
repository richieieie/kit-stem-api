using kit_stem_api.Data;
using kit_stem_api.Models.Domain;
using kit_stem_api.Models.DTO;
using kit_stem_api.Repositories.IRepositories;
using kit_stem_api.Services.IServices;
using kit_stem_api.Repositories;

namespace kit_stem_api.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly UnitOfWork _unitOfWork;
        public CategoryService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse> CreateAsync(CategoryCreateDTO categoryCreateDTO)
        {
            try
            {
                var newCategory = new KitsCategory()
                {
                    Name = categoryCreateDTO.Name,
                    Description = categoryCreateDTO.Description!,
                    Status = true
                };
                await _unitOfWork.CategoryRepository.CreateAsync(newCategory);
                return new ServiceResponse()
                            .SetSucceeded(true)
                            .AddDetail("message", "Tạo mới loại kit mới thành công!");
            }
            catch
            {
                return new ServiceResponse()
                            .SetSucceeded(false)
                            .AddDetail("message", "Tạo loại kit mới thất bại!")
                            .AddError("outOfService", "Không thể tạo mới loại kit ngay lúc ngày!");
            }

        }

        public async Task<ServiceResponse> RemoveByIdAsync(int id)
        {
            try
            {
                var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
                if (category == null)
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Xóa loại kit thất bại!")
                        .AddError("notFound", "Không tìm thấy loại kit!");
                }
                category.Status = false;
                await _unitOfWork.CategoryRepository.UpdateAsync(category);
                return new ServiceResponse()
                            .SetSucceeded(true)
                            .AddDetail("message", "Xóa một loại kit thành công!");
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Xóa loại kit thất bại!")
                    .AddError("outOfService", "Không thể xóa loại kit ngay lúc này!");
            }

        }

        public async Task<ServiceResponse> GetAsync()
        {
            try
            {
                var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
                return new ServiceResponse()
                            .SetSucceeded(true)
                            .AddDetail("message", "Lấy danh sách loại kit thành công!")
                            .AddDetail("data", new { categories });
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Lấy danh sách loại kit thất bại!")
                    .AddError("outOfService", "Không thể tạo mới một loại kit ngay lúc này!");
            }

        }

        public async Task<ServiceResponse> UpdateAsync(CategoryUpdateDTO categoryUpdateDTO)
        {
            try
            {
                var category = new KitsCategory()
                {
                    Id = categoryUpdateDTO.Id,
                    Name = categoryUpdateDTO.Name,
                    Description = categoryUpdateDTO.Description!
                };
                await _unitOfWork.CategoryRepository.UpdateAsync(category);
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Chỉnh sửa một loại kit thành công!");
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Chỉnh sửa loại kit thất bại!")
                    .AddError("outOfService", "Không thể chỉnh sửa một loại kit ngay lúc này!");
            }
        }

        public async Task<ServiceResponse> GetByIdAsync(int id)
        {
            try
            {
                var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
                if (category == null)
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Lấy thông tin loại kit thất bại!")
                        .AddError("notFound", "Không tìm thấy loại kit!");
                }
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("data", new { category });
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Lấy thông tin loại kit thất bại")
                    .AddError("outOfService", "Không thể lấy thông tin loại kit ngay lúc này!");
            }

        }
    }
}
