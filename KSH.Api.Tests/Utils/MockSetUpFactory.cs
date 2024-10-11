using AutoMapper;
using KST.Api.Data;
using KST.Api.Models.Domain;
using KST.Api.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;

namespace KSH.Api.Tests.Utils
{
    public static class MockSetUpFactory
    {
        private static Mock<KitStemDbContext>? dbContextMock;
        private static Mock<UnitOfWork>? unitOfWorkMock;
        private static Mock<IMapper>? _mapperMock;
        private static Mock<UserManager<ApplicationUser>>? _userManagerMock;
        public static Mock<KitStemDbContext> GetDbContextMock()
        {
            if (dbContextMock == null)
            {
                var options = new DbContextOptionsBuilder<KitStemDbContext>().Options;
                dbContextMock = new Mock<KitStemDbContext>(options);
                var transactionMock = new Mock<IDbContextTransaction>();
                var dbFacade = new Mock<DatabaseFacade>(dbContextMock.Object);
                dbContextMock.Setup(dbCtx => dbCtx.Database).Returns(dbFacade.Object);
                dbContextMock.Setup(dbCtx => dbCtx.Database.BeginTransactionAsync(It.IsAny<CancellationToken>())).ReturnsAsync(transactionMock.Object);
                transactionMock.Setup(t => t.CommitAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
                transactionMock.Setup(t => t.RollbackAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            }
            return dbContextMock;
        }

        public static Mock<UnitOfWork> GetUnitOfWorkMock()
        {
            if (unitOfWorkMock == null)
            {
                var labRepositoryMock = new Mock<LabRepository>(GetDbContextMock().Object);
                unitOfWorkMock = new Mock<UnitOfWork>(GetDbContextMock().Object);
                unitOfWorkMock.Setup(u => u.LabRepository).Returns(labRepositoryMock.Object);
            }
            return unitOfWorkMock;
        }

        public static Mock<IMapper> GetMapperMock()
        {
            _mapperMock ??= new Mock<IMapper>();
            return _mapperMock;
        }

        public static Mock<UserManager<ApplicationUser>> GetUserManagerMock()
        {
            if (_userManagerMock == null)
            {
                var userStore = new Mock<IUserStore<ApplicationUser>>();
                _userManagerMock = new Mock<UserManager<ApplicationUser>>(userStore.Object, null, null, null, null, null, null, null, null);
            }
            return _userManagerMock;
        }
    }
}