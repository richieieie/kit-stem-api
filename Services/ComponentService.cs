﻿using kit_stem_api.Data;
using kit_stem_api.Models.Domain;
using kit_stem_api.Models.DTO;
using kit_stem_api.Repositories;
using kit_stem_api.Repositories.IRepositories;
using kit_stem_api.Services.IServices;

namespace kit_stem_api.Services
{
    public class ComponentService : IComponentService
    {

        private readonly UnitOfWork _unitOfWork;
        public ComponentService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse> CreateAsync(ComponentCreateDTO component)
        {
            try
            {
                var newComponent = new Component()
                {
                    TypeId = component.TypeId,
                    Name = component.Name
                };
                await _unitOfWork.ComponentRepository.CreateAsync(newComponent);
                return new ServiceResponse()
                            .SetSucceeded(true)
                            .AddDetail("message", "Tạo mới linh kiện thành công!");
            } 
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Tạo mới linh kiện thất bại!")
                    .AddError("outOfService", "Không thể tạo mới linh kiện ngay lúc này!");
            }
        }

        public async Task<ServiceResponse> RemoveAsync(int id)
        {
            try
            {
                var component = new Component()
                {
                    Id = id,
                };
                await _unitOfWork.ComponentRepository.RemoveAsync(component);
                return new ServiceResponse()
                            .SetSucceeded(true)
                            .AddDetail("message", "Xóa linh kiện thành công!");
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Xóa linh kiện thất bại!")
                    .AddError("outOfService", "Không thể xóa linh kiện ngay lúc này!");
            }
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            try
            {
                var components = await _unitOfWork.ComponentRepository.GetAllAsync();
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Lấy danh sách linh kiện thành công!")
                    .AddDetail("data", new { components });
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Lấy danh sách thành phần thất bại!")
                    .AddError("outOfService", "Không thể lấy danh sách thành phần ngày lúc này!");
            }
        }

        public async Task<ServiceResponse> UpdateAsync(ComponentUpdateDTO component)
        {
            try
            {
                var updateComponent = new Component()
                {
                    Id = component.Id,
                    TypeId = component.TypeId,
                    Name = component.Name
                };
                await _unitOfWork.ComponentRepository.UpdateAsync(updateComponent);
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Chỉnh sửa linh kiện thành công!");
            }
            catch
            {
                return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Chỉnh sửa linh kiện thất bại!")
                        .AddError("outOfSercive", "Không thể chỉnh sửa linh kiện ngay lúc này!");
            }
        }
    }
}