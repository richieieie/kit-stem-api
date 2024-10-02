﻿using kit_stem_api.Models.Domain;
using kit_stem_api.Models.DTO;
using kit_stem_api.Repositories;
using kit_stem_api.Services.IServices;

namespace kit_stem_api.Services
{
    public class LevelService : ILevelService
    {
        private readonly UnitOfWork _unitOfWork;

        public LevelService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ServiceResponse> CreateAsync(LevelCreateDTO level)
        {
            try
            {
                var newLevel = new Level()
                {
                    Name = level.Name,
                    Status = true,
                };
                await _unitOfWork.LevelRepository.CreateAsync(newLevel);
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Tạo mới một level thành công!");
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Tạo mới một level thất bại!")
                    .AddError("outOfService", "Không thể tạo mới một level ngay lúc này!");
            }
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            try
            {
                var levels = await _unitOfWork.LevelRepository.GetAllAsync();
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Lấy danh sách level thành công!")
                    .AddDetail("data", new { levels });
            }
            catch
            {
                return new ServiceResponse()
                    .AddDetail("message", "Lấy danh sách level thất bại!")
                    .AddError("outOfService", "Không thể lấy danh sách level ngay lúc này!");
            }
        }

        public async Task<ServiceResponse> GetByIdAsync(int id)
        {
            try
            {
                var level = await _unitOfWork.LevelRepository.GetByIdAsync(id);
                if (level == null)
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Lấy thông tin level thất bại!")
                        .AddError("notFound", "Không tìm thấy level!");
                }
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("data", new { level });
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Lấy thông tin linh kiện thất bại")
                    .AddError("outOfService", "Không thể lấy thông tin linh kiện ngay lúc này!");
            }
        }

        public async Task<ServiceResponse> RemoveByIdAsync(int id)
        {
            try
            {
                var level = await _unitOfWork.LevelRepository.GetByIdAsync(id);
                if (level == null)
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Chỉnh sửa level không thành công!")
                        .AddError("notFound", "Không thể tìm thấy level ngay lúc này!");
                }
                level.Status = false;
                await _unitOfWork.LevelRepository.UpdateAsync(level);
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Xóa một level thành công!");
            }
            catch 
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Xóa một level thất bại!")
                    .AddError("outOfService", "Không thể xóa level ngay lúc này!");
            }
        }

        public async Task<ServiceResponse> RestoreByIdAsync(int id)
        {
            try
            {
                var level = await _unitOfWork.LevelRepository.GetByIdAsync(id);
                if (level == null)
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Chỉnh sửa level không thành công!")
                        .AddError("notFound", "Không thể tìm thấy level ngay lúc này!");
                }
                level.Status = true;
                await _unitOfWork.LevelRepository.UpdateAsync(level);
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Xóa một level thành công!");
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Xóa một level thất bại!")
                    .AddError("outOfService", "Không thể xóa level ngay lúc này!");
            }
        }

        public async Task<ServiceResponse> UpdateAsync(LevelUpdateDTO level)
        {
            try
            {
                var updateLevel = new Level()
                {
                    Id = level.Id,
                    Name = level.Name,
                    Status = true
                };
                await _unitOfWork.LevelRepository.UpdateAsync(updateLevel);
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Chỉnh sửa level thành công!");
            } 
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Chỉnh sửa level thất bại!")
                    .AddError("outOfService", "Không thể chỉnh sửa level ngay lúc này!");
            }
        }
    }
}
