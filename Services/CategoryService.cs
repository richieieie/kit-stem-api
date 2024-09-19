using kit_stem_api.Data;
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

        public async Task<ServiceResponse> GetCategoriesAsync()
        {
            var categories = await _categoryRepository.GetCategoriesAsync();
            return new ServiceResponse()
                        .SetSucceeded(true)
                        .AddDetail("Date", categories);
        }
    }
}
