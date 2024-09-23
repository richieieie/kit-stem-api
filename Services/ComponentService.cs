using kit_stem_api.Data;
using kit_stem_api.Models.Domain;
using kit_stem_api.Models.DTO;
using kit_stem_api.Repositories;
using kit_stem_api.Repositories.IRepositories;
using kit_stem_api.Services.IServices;

namespace kit_stem_api.Services
{
    public class ComponentService : IComponentService
    {
        private readonly IComponentRepository _componentRepository;
        private readonly KitStemDbContext _dbContext;

        public ComponentService(IComponentRepository componentRepository, KitStemDbContext dbContext)
        {
            _componentRepository = componentRepository;
            _dbContext = dbContext;
        }

        public async Task<ServiceResponse> CreateComponentAsync(ComponentCreateDTO component)
        {
            try
            {
                var alreadyType = await _dbContext.ComponentsTypes.FindAsync(component.TypeId);
                if (alreadyType == null)
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Tạo mới thành phần thất bại!")
                        .AddError("notFoundTypeId", "Không tìm thấy TypeId ngay lúc ngày!");
                }

                var newComponent = new Component()
                {
                    TypeId = alreadyType.Id,
                    Name = component.Name,
                };
                newComponent = await _componentRepository.CreateComponentAsync(newComponent);
                return new ServiceResponse()
                            .SetSucceeded(true)
                            .AddDetail("NewComponent", newComponent);
            } 
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Tạo mới thành phần thất bại!")
                    .AddError("unhandledExeption", "Không thể tạo mới thành phần ngay lúc này!");
            }
        }

        public async Task<ServiceResponse> DeleteComponentAsync(int Id)
        {
            try
            {
                var component = await _componentRepository.DeleteComponentAsync(Id);
                return new ServiceResponse()
                            .SetSucceeded(true)
                            .AddDetail("deleteComponent", component);
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Xóa thành phần thất bại!")
                    .AddError("unhandledException", "Không thể xóa thành phần ngay lúc này!");
            }
        }

        public async Task<ServiceResponse> GetComponentsAsync()
        {
            try
            {
                var components = await _componentRepository.GetComponentsAsync();
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("components", components);
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Lấy danh sách thành phần thất bại!")
                    .AddError("unhandledExeption", "Không thể lấy danh sách thành phần ngày lúc này!");
            }
        }

        public async Task<ServiceResponse> UpdateComponentAsync(int Id, ComponentUpdateDTO component)
        {
            try
            {
                var alreadyType = await _dbContext.ComponentsTypes.FindAsync(component.TypeId);
                if (alreadyType == null)
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Chỉnh sửa thành phần thất bại!")
                        .AddError("notFoundTypeId", "Không tìm thấy TypeId ngay lúc ngày!");
                }
                var updateComponent = await _componentRepository.UpdateComponentAsync(Id, component);
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("updateComponent", updateComponent);
            }
            catch
            {
                return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Chỉnh sửa thành phần thất bại!")
                        .AddError("unhandledExeption", "Không thể chỉnh sửa thành phần ngay lúc này!");
            }
        }
    }
}
