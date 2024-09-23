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

        public async Task<ServiceResponse> AddCategoriesAsync(CategoryCreateDTO categoryCreateDTO)
        {
            try
            {
                var newCategory = new KitsCategory()
                {
                    Name = categoryCreateDTO.Name,
                    Description = categoryCreateDTO.Description,
                };
                newCategory = await _categoryRepository.AddCategoryAsync(newCategory);
                return new ServiceResponse()
                            .SetSucceeded(true)
                            .AddDetail("NewCategory", newCategory);
            }
            catch
            {
                return new ServiceResponse()
                            .SetSucceeded(false)
                            .AddDetail("message", "Tạo loại kit thất bại!")
                            .AddError("unhandledException", "Không thể tạo mới loại kit ngay lúc ngày!");
            }
            
        }

        public async Task<ServiceResponse> DeleteCategoriesAsync(int Id)
        {
            try
            {
                var category = await _categoryRepository.DeleteCategoryAsync(Id);
                return new ServiceResponse()
                            .SetSucceeded(true)
                            .AddDetail("deleteCategory", category);
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Xóa loại kit thất bại!")
                    .AddError("unhandledException", "Không thể xóa loại kit ngay lúc này!");
            }

        }

        public async Task<ServiceResponse> GetCategoriesAsync()
        {
            try
            {
                var categories = await _categoryRepository.GetCategoriesAsync();
                return new ServiceResponse()
                            .SetSucceeded(true)
                            .AddDetail("categories", categories);
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Lấy danh sách loại kit thất bại!")
                    .AddError("unhandledException", "Không thể tạo mới một loại kit ngay lúc này!");
            }

        }

        public async Task<ServiceResponse> UpdateCategoriesAsync(int Id, CategoryUpdateDTO categoryUpdateDTO)
        {
            try
            {
                var updateCategory = await _categoryRepository.UpdateCategoryAsync(Id, categoryUpdateDTO);
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("updateCategory", updateCategory);
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Chỉnh sửa loại kit thất bại")
                    .AddError("unhandledException", "Không thể chỉnh sửa một loại kit ngay lúc này!");
            }
        }
    }
}
