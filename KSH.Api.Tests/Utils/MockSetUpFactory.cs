using AutoMapper;
using KST.Api.Data;
using KST.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace KSH.Api.Tests.Utils
{
    public static class MockSetUpFactory
    {
        private static Mock<KitStemDbContext>? dbContextMock;
        private static Mock<UnitOfWork>? unitOfWorkMock;
        private static Mock<IMapper>? _mapperMock;
        public static Mock<KitStemDbContext> GetDbContextMock()
        {
            if (dbContextMock == null)
            {
                var options = new DbContextOptionsBuilder<KitStemDbContext>().Options;
                dbContextMock = new Mock<KitStemDbContext>(options);
            }
            return dbContextMock;
        }

        public static Mock<UnitOfWork> GetUnitOfWorkMock()
        {
            if (unitOfWorkMock == null)
            {
                unitOfWorkMock = new Mock<UnitOfWork>(GetDbContextMock().Object);

                var labRepositoryMock = new Mock<LabRepository>(GetDbContextMock().Object);
                unitOfWorkMock.Setup(u => u.LabRepository).Returns(labRepositoryMock.Object);

                var typeRepositoryMock = new Mock<ComponentTypeRepository>(GetDbContextMock().Object);
                unitOfWorkMock.Setup(u => u.ComponentTypeRepository).Returns(typeRepositoryMock.Object);
            }
            return unitOfWorkMock;
        }

        public static Mock<IMapper> GetMapperMock()
        {
            _mapperMock ??= new Mock<IMapper>();
            return _mapperMock;
        }
    }
}