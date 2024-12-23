﻿using AutoMapper;
using KSH.Api.Tests.Utils;
using KSH.Api.Models.Domain;
using KSH.Api.Models.DTO;
using KSH.Api.Repositories;
using KSH.Api.Services;
using Moq;
using Allure.XUnit;
using Xunit;
using Allure.Xunit.Attributes;
using Allure.Xunit.Attributes.Steps;
namespace KSH.Api.Tests
{
    [AllureFeature("Type API")]
    public class TypeServiceTests
    {
        private Mock<UnitOfWork> _unitOfWorkMock;
        private Mock<IMapper> _mapperMock;
        private ComponentTypeService _typeService;

        [AllureBefore("Setup API connection")]
        public TypeServiceTests()
        {
            _unitOfWorkMock = MockSetUpFactory.GetUnitOfWorkMock();
            _mapperMock = MockSetUpFactory.GetMapperMock();
            _typeService = new ComponentTypeService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        [AllureTag("create")]
        public async Task CreateAsync_ValidCredentials_ReturnTrue()
        {
            //Arrange
            var typeDTO = new ComponentTypeCreateDTO();
            var type = new ComponentsType();
            _mapperMock.Setup(m => m.Map<ComponentsType>(typeDTO)).Returns(type);
            _unitOfWorkMock.Setup(u => u.ComponentTypeRepository.CreateAsync(type)).ReturnsAsync(true);

            //Act
            var response = await _typeService.CreateAsync(typeDTO);

            //Assert
            Assert.True(response.Succeeded);
            Assert.Equal("Tạo mới loại linh kiện thành công!", response.GetDetailsValue("message"));
        }

        [Fact]
        [AllureTag("remove")]
        public async Task RemoveAsync_ValidCredentials_ReturnTrue()
        {
            // Arrange
            var typeId = 1;
            var type = new ComponentsType { Id = typeId, Status = false };

            _unitOfWorkMock.Setup(u => u.ComponentTypeRepository.GetByIdAsync(typeId)).ReturnsAsync(type);
            _unitOfWorkMock.Setup(u => u.ComponentTypeRepository.UpdateAsync(type)).ReturnsAsync(true);

            // Act
            var response = await _typeService.RemoveByIdAsync(typeId);

            // Assert
            Assert.True(response.Succeeded);
            Assert.Equal("Xóa một loại linh kiện thành công!", response.GetDetailsValue("message"));
        }

        [Fact]
        [AllureTag("update")]
        public async Task UpdateAsync_ValidCredentials_ReturnTrue()
        {
            // Arrange
            var typeDTO = new ComponentTypeUpdateDTO();
            var type = new ComponentsType();

            _unitOfWorkMock.Setup(u => u.ComponentTypeRepository.GetByIdAsync(typeDTO.Id)).ReturnsAsync(type);
            _mapperMock.Setup(m => m.Map<ComponentsType>(typeDTO)).Returns(type);
            _unitOfWorkMock.Setup(u => u.ComponentTypeRepository.UpdateAsync(type)).ReturnsAsync(true);

            // Act
            var response = await _typeService.UpdateAsync(typeDTO);

            // Assert
            Assert.True(response.Succeeded);
            Assert.Equal("Chỉnh sửa một loại linh kiện thành công!", response.GetDetailsValue("message"));
        }

    }
}
