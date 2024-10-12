using KSH.Api.Data;

namespace KSH.Api.Repositories
{
        public class UnitOfWork
        {
                public readonly KitStemDbContext _dbContext;
                public virtual UserRepository UserRepository { get; set; }
                public virtual LabRepository LabRepository { get; set; }
                public virtual CategoryRepository CategoryRepository { get; set; }
                public virtual ComponentTypeRepository ComponentTypeRepository { get; set; }
                public virtual ComponentRepository ComponentRepository { get; set; }
                public virtual LevelRepository LevelRepository { get; set; }
                public virtual PackageRepository PackageRepository { get; set; }
                public virtual KitRepository KitRepository { get; set; }
                public virtual PackageLabRepository PackageLabRepository { get; set; }
                public virtual OrderRepository OrderRepository { get; set; }
                public virtual CartRepository CartRepository { get; set; }
                public virtual KitImageRepository KitImageRepository { get; set; }
                public virtual PaymentRepository PaymentRepository { get; set; }
                public virtual OrderSupportRepository OrderSupportRepository { get; set; }

                public UnitOfWork(KitStemDbContext dbContext)
                {
                        _dbContext = dbContext;
                        UserRepository = new UserRepository(_dbContext);
                        LabRepository = new LabRepository(_dbContext);
                        CategoryRepository = new CategoryRepository(_dbContext);
                        ComponentTypeRepository = new ComponentTypeRepository(_dbContext);
                        ComponentRepository = new ComponentRepository(_dbContext);
                        LevelRepository = new LevelRepository(_dbContext);
                        PackageRepository = new PackageRepository(_dbContext);
                        PackageLabRepository = new PackageLabRepository(_dbContext);
                        KitRepository = new KitRepository(_dbContext);
                        OrderRepository = new OrderRepository(_dbContext);
                        CartRepository = new CartRepository(_dbContext);
                        KitImageRepository = new KitImageRepository(_dbContext);
                        PaymentRepository = new PaymentRepository(_dbContext);
                        OrderSupportRepository = new OrderSupportRepository(_dbContext);
                }
        }
}
