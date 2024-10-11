using AutoMapper;
using KSH.Api.Tests.Utils;
using KST.Api.Repositories;
using KST.Api.Services;
using Moq;

namespace KSH.Api.Tests
{
    public class CategoryServiceTests
    {
        private Mock<UnitOfWork> _unitOfWorkMock;
        private Mock<IMapper> _mapperMock;
        private CategoryService _categoryServiceMock;

        public CategoryServiceTests()
        {
            _unitOfWorkMock = MockSetUpFactory.GetUnitOfWorkMock();
            _mapperMock = MockSetUpFactory.GetMapperMock();
            _categoryServiceMock = new CategoryService(_unitOfWorkMock.Object, _mapperMock.Object);
        }
    }
}
