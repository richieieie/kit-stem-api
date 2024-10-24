using AutoMapper;
using KSH.Api.Tests.Utils;
using KSH.Api.Models.Domain;
using KSH.Api.Models.DTO;
using KSH.Api.Models.DTO.Response;
using KSH.Api.Repositories;
using KSH.Api.Services;
using Moq;
using Allure.Xunit.Attributes;

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
        [AllureOwner("Trung Nguyen")]
        [AllureDescription("This is the test for creating a new lab with valid information and returning success result")]
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

        [Fact]
        public async Task UpdateAsync_ValidCredentials_ReturnTrue()
        {
            // Arrange
            var labUpdateDTO = new LabUpdateDTO() { LevelId = 1, KitId = 1, Name = "Name", Author = "Author", Price = 1, MaxSupportTimes = 1 };
            var lab = new Lab();
            var url = "Labs/default.pdf";
            _unitOfWorkMock.Setup(u => u.LabRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(lab);
            _unitOfWorkMock.Setup(u => u.LabRepository.UpdateAsync(lab)).ReturnsAsync(true);

            //Act
            var response = await _labService.UpdateAsync(labUpdateDTO, url);

            //Assert
            Assert.True(response.Succeeded);
            Assert.Equal("Chỉnh sửa bài lab thành công!", response.GetDetailsValue("message"));
            Assert.Equal(labUpdateDTO.LevelId, lab.LevelId);
            Assert.Equal(labUpdateDTO.KitId, lab.KitId);
            Assert.Equal(labUpdateDTO.Name, lab.Name);
            Assert.Equal(labUpdateDTO.Author, lab.Author);
            Assert.Equal(labUpdateDTO.Price, lab.Price);
            Assert.Equal(labUpdateDTO.MaxSupportTimes, lab.MaxSupportTimes);
        }

        [Fact]
        public async Task UpdateAsync_InValidCredentials_ReturnFalse()
        {
            // Arrange
            var labUpdateDTO = new LabUpdateDTO();
            var lab = new Lab();
            var url = "Labs/default.pdf";
            _unitOfWorkMock.Setup(u => u.LabRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Lab?)null);

            //Act
            var response = await _labService.UpdateAsync(labUpdateDTO, url);

            //Assert
            var errors = response.GetErrors();
            Assert.False(response.Succeeded);
            Assert.Equal("Chỉnh sửa bài lab thất bại!", response.GetDetailsValue("message"));
            Assert.Equal("Không tìm thấy bài lab để chỉnh sửa!", errors!["invalid-credentials"]);
        }

        [Fact]
        public async Task GetByIdAsync_IdExists_ReturnTrue()
        {
            //Arrange
            _unitOfWorkMock.Setup(u => u.LabRepository.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Lab());
            _mapperMock.Setup(m => m.Map<LabResponseDTO>(It.IsAny<Lab>())).Returns(new LabResponseDTO());

            //Act
            var response = await _labService.GetByIdAsync(It.IsAny<Guid>());

            //Assert
            var data = response.GetDetailsValue("data");
            Assert.True(response.Succeeded);
            Assert.Equal("Lấy thông tin bài lab thành công!", response.GetDetailsValue("message"));
            Assert.NotNull(data!.GetType().GetProperty("lab")?.GetValue(data));
        }
    }
}