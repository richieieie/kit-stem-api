using AutoMapper;
using KSH.Api.Tests.Utils;
using KST.Api.Models.Domain;
using KST.Api.Models.DTO;
using KST.Api.Repositories;
using KST.Api.Services;
using Moq;

namespace KSH.Api.Tests
{
    public class LabServiceTests
    {

        private Mock<UnitOfWork> _unitOfWorkMock;
        private Mock<IMapper> _mapperMock;
        private LabService _labService;
        public LabServiceTests()
        {
            _unitOfWorkMock = MockSetUpFactory.GetUnitOfWorkMock();
            _mapperMock = MockSetUpFactory.GetMapperMock();
            _labService = new LabService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ValidCredentials_ReturnTrue()
        {
            //Arrange
            var labUploadDTO = new LabUploadDTO();
            var lab = new Lab();
            var id = Guid.NewGuid();
            var url = "Labs/default.pdf";
            _mapperMock.Setup(m => m.Map<Lab>(labUploadDTO)).Returns(lab);
            _unitOfWorkMock.Setup(u => u.LabRepository.CreateAsync(lab)).ReturnsAsync(true);

            //Act
            var response = await _labService.CreateAsync(labUploadDTO, id, url);

            //Assert
            Assert.True(response.Succeeded);
            Assert.Equal("Thêm mới bài lab thành công!", response.GetDetailsValue("message"));
        }
    }
}