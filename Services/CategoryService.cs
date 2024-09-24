using kit_stem_api.Data;
using kit_stem_api.Models.Domain;
using kit_stem_api.Models.DTO;
using kit_stem_api.Repositories.IRepositories;
using kit_stem_api.Services.IServices;

namespace kit_stem_api.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<ServiceResponse> CreateAsync(CategoryCreateDTO categoryCreateDTO)
        {
            try
            {
                var newCategory = new KitsCategory()
                {
                    Name = categoryCreateDTO.Name,
                    Description = categoryCreateDTO.Description!,
                };
                await _categoryRepository.AddAsync(newCategory);
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

        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            try
            {
                var category = new KitsCategory()
                {
                    Id = id,
                };
                await _categoryRepository.DeleteAsync(category);
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
                var categories = await _categoryRepository.GetAsync();
                return new ServiceResponse()
                            .SetSucceeded(true)
                            .AddDetail("data", new {categories});
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
                bool updateCategory = await _categoryRepository.UpdateAsync(category);
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("data", "Chỉnh sửa một loại kit thành công!");
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Chỉnh sửa loại kit thất bại!")
                    .AddError("unhandledException", "Không thể chỉnh sửa một loại kit ngay lúc này!");
            }
        }
    }
}
