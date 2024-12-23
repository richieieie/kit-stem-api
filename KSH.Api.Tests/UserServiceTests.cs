using AutoMapper;
using KSH.Api.Tests.Utils;
using KSH.Api.Data;
using KSH.Api.Models.Domain;
using KSH.Api.Models.DTO;
using KSH.Api.Repositories.IRepositories;
using KSH.Api.Services;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace KSH.Api.Tests
{
    public class UserServiceTests
    {
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private Mock<ITokenRepository> _tokenRepositoryMock;
        private Mock<KitStemDbContext> _dbContextMock;
        private Mock<IMapper> _mapper;
        private UserService _userService;
        public UserServiceTests()
        {
            _dbContextMock = MockSetUpFactory.GetDbContextMock();

            _userManagerMock = MockSetUpFactory.GetUserManagerMock();

            _tokenRepositoryMock = new Mock<ITokenRepository>();

            _mapper = new Mock<IMapper>();

            _userService = new UserService(_dbContextMock.Object, _userManagerMock.Object, _tokenRepositoryMock.Object, _mapper.Object);
        }

        //[Fact]
        //public async Task LoginAsync_ValidCredentials_ReturnSucceed()
        //{
        //    //Arrange
        //    var requestBody = new UserLoginDTO()
        //    {
        //        Email = "test@example.com",
        //        Password = "12345aA@"
        //    };
        //    var user = new ApplicationUser()
        //    {
        //        Email = "test@example.com",
        //        UserName = "test@example.com",
        //        Status = true,
        //    };
        //    var role = "customer";
        //    var accessToken = "new jwt access token";
        //    var refreshToken = new RefreshToken()
        //    {
        //        Id = Guid.NewGuid()
        //    };
        //    _userManagerMock.Setup(um => um.FindByNameAsync(requestBody.Email)).ReturnsAsync(user);
        //    _userManagerMock.Setup(um => um.CheckPasswordAsync(user, requestBody.Password)).ReturnsAsync(true);
        //    _userManagerMock.Setup(um => um.GetRolesAsync(user)).ReturnsAsync([role]);
        //    _tokenRepositoryMock.Setup(tr => tr.CreateOrUpdateRefreshTokenAsync(user)).ReturnsAsync(refreshToken);
        //    _tokenRepositoryMock.Setup(tr => tr.GenerateJwtToken(user, role)).Returns(accessToken);

        //    //Act
        //    var response = await _userService.LoginAsync(requestBody);

        //    //Assert
        //    Assert.True(response.Succeeded);
        //    Assert.Equal("Đăng nhập thành công!", response.GetDetailsValue("message"));
        //    Assert.Equal("new jwt access token", response.GetDetailsValue("accessToken"));
        //    Assert.Equal(refreshToken.Id, response.GetDetailsValue("refreshToken"));
        //}

        [Fact]
        public async Task RegisterAsync_ValidCredentials_ReturnSucceed()
        {
            //Arrange
            var requestBody = new UserRegisterDTO();
            var role = "customer";
            var expectedToken = "verify-token";
            _userManagerMock.Setup(um => um.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser?)null);
            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(um => um.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(expectedToken);

            //Act
            var (response, token) = await _userService.RegisterAsync(requestBody, role);

            //Assert
            Assert.True(response.Succeeded);
            Assert.Equal(expectedToken, token);
            Assert.Equal("Tạo mới tài khoản thành công!", response.GetDetailsValue("message"));
        }

        [Fact]
        public async Task RegisterAsync_InValidCredentials_ReturnFalse()
        {
            //Arrange
            var requestBody = new UserRegisterDTO()
            {
                Email = "test@example.com",
                Password = "12345aA@"
            };
            var existedUser = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString()
            };
            string role = "customer";
            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed());

            //Act
            var (response, token) = await _userService.RegisterAsync(requestBody, role);

            //Assert
            var errors = (Dictionary<string, string>)response.GetDetailsValue("errors")!;
            Assert.False(response.Succeeded);
            Assert.Equal("Tạo tài khoản thất bại!", response.GetDetailsValue("message"));
            Assert.Equal("Tên tài khoản đã tồn tại!", errors["unavailable-username"]);
        }

        [Fact]
        public async Task RefreshTokenAsync_ValidRefreshTokenId_ReturnTrue()
        {
            // Arrange
            var user = new ApplicationUser { Id = "user-id", UserName = "test@example.com", Status = true };
            var refreshTokenId = Guid.NewGuid();
            var refreshToken = new RefreshToken { Id = Guid.NewGuid(), UserId = user.Id };

            _tokenRepositoryMock.Setup(tr => tr.GetUserIdByRefreshTokenAsync(refreshTokenId)).ReturnsAsync(user.Id);
            _userManagerMock.Setup(um => um.FindByIdAsync(user.Id)).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.GetRolesAsync(user)).ReturnsAsync(new List<string> { "customer" });
            _tokenRepositoryMock.Setup(tr => tr.CreateOrUpdateRefreshTokenAsync(user)).ReturnsAsync(refreshToken);
            _tokenRepositoryMock.Setup(tr => tr.GenerateJwtToken(user, "customer")).Returns("New JWT Access Token");

            //Act
            var response = await _userService.RefreshTokenAsync(refreshTokenId);

            //Assert
            Assert.True(response.Succeeded);
        }
    }
}