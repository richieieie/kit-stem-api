﻿using KST.Api.Models.Domain;
using KST.Api.Models.DTO;
using KST.Api.Repositories;
using KST.Api.Repositories.IRepositories;
using KST.Api.Services.IServices;


namespace KST.Api.Services
{
    public class ComponentTypeService : IComponentTypeService
    {
        private readonly UnitOfWork _unitOfWork;

        public ComponentTypeService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse> CreateAsync(ComponentTypeCreateDTO componentTypeCreateDTO)
        {
            try
            {
                var newComponentType = new ComponentsType()
                {
                    Name = componentTypeCreateDTO.Name,
                    Status = true
                };
                await _unitOfWork.ComponentTypeRepository.CreateAsync(newComponentType);
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Tạo mới một linh kiện thành công!");
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Tạo loại thành phần thất bại!")
                    .AddError("unhandledExeption", "Không thể tạo loại thành phần ngay lúc này!");
            }

        }

        public async Task<ServiceResponse> RemoveByIdAsync(int id)
        {
            try
            {
                var type = await _unitOfWork.ComponentTypeRepository.GetByIdAsync(id);
                if (type == null)
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .SetStatusCode(StatusCodes.Status404NotFound)
                        .AddDetail("message", "Xóa loại linh kiện thất bại!")
                        .AddError("notFound", "Không tìm thấy loại linh kiện!");
                }
                type.Status = false;
                await _unitOfWork.ComponentTypeRepository.UpdateAsync(type);
                return new ServiceResponse()
                            .SetSucceeded(true)
                            .AddDetail("message", "Xóa một loại linh kiện thành công!");
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Xóa loại linh kiện thất bại!")
                    .AddError("outOfService", "Không thể xóa loại linh kiện ngay lúc này!");
            }
        }

        public async Task<ServiceResponse> RestoreByIdAsync(int id)
        {
            try
            {
                var type = await _unitOfWork.ComponentTypeRepository.GetByIdAsync(id);
                if (type == null)
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .SetStatusCode(StatusCodes.Status404NotFound)
                        .AddDetail("message", "Xóa loại linh kiện thất bại!")
                        .AddError("notFound", "Không tìm thấy loại linh kiện!");
                }
                type.Status = true;
                await _unitOfWork.ComponentTypeRepository.UpdateAsync(type);
                return new ServiceResponse()
                            .SetSucceeded(true)
                            .AddDetail("message", "Xóa một loại linh kiện thành công!");
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Xóa loại linh kiện thất bại!")
                    .AddError("outOfService", "Không thể xóa loại linh kiện ngay lúc này!");
            }
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            try
            {
                var componentTypes = await _unitOfWork.ComponentTypeRepository.GetAllAsync();
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("data", new { componentTypes });
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Lấy danh sách các loại linh kiện thất bại!")
                    .AddError("outOfService", "Không thể lấy danh sách loại linh kiện ngày lúc này!");
            }
        }

        public async Task<ServiceResponse> UpdateAsync(ComponentTypeUpdateDTO componentTypeUpdateDTO)
        {
            try
            {
                var type = new ComponentsType()
                {
                    Id = componentTypeUpdateDTO.Id,
                    Name = componentTypeUpdateDTO.Name,
                    Status = true,
                };
                var allreadyType = await _unitOfWork.ComponentTypeRepository.GetByIdAsync(type.Id);
                if (allreadyType == null)
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .SetStatusCode(StatusCodes.Status404NotFound)
                        .AddDetail("message", "Chỉnh sửa loại linh kiện thất bại!")
                        .AddError("notFound", "Không tìm thấy loại linh kiện!");
                }
                await _unitOfWork.ComponentTypeRepository.UpdateAsync(type);
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Chỉnh sửa một loại linh kiện thành công!");
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Chỉnh sửa loại linh kiện thất bại!")
                    .AddError("outOfService", "Không thể chỉnh sửa loại linh kiện ngay lúc này!");
            }
        }

        public async Task<ServiceResponse> GetByIdAsync(int id)
        {
            try
            {
                var type = await _unitOfWork.ComponentTypeRepository.GetByIdAsync(id);
                if (type == null)
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .SetStatusCode(StatusCodes.Status404NotFound)
                        .AddDetail("message", "Lấy thông tin loại linh kiện thất bại!")
                        .AddError("notFound", "Không tìm thấy loại linh kiện!");
                }
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("data", new { type });
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Lấy thông tin loại linh kiện thất bại")
                    .AddError("outOfService", "Không thể lấy thông tin loại linh kiện ngay lúc này!");
            }
        }
    }
}
