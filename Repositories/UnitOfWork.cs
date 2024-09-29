using kit_stem_api.Data;
using kit_stem_api.Models.Domain;
using kit_stem_api.Repositories;

namespace kit_stem_api.Repositories
{
        public class UnitOfWork
        {
                public readonly KitStemDbContext _dbContext;
                public LabRepository LabRepository { get; }
                public CategoryRepository CategoryRepository { get; }
                public ComponentTypeRepository ComponentTypeRepository { get; }
                public ComponentRepository ComponentRepository { get; }
                public LevelRepository LevelRepository { get; }
                public PackageRepository PackageRepository { get; }
                public PackageLabRepository PackageLabRepository { get; }
                public UnitOfWork(KitStemDbContext dbContext)
                {
                        _dbContext = dbContext;
                        LabRepository = new LabRepository(_dbContext);
                        CategoryRepository = new CategoryRepository(_dbContext);
                        ComponentTypeRepository = new ComponentTypeRepository(_dbContext);
                        ComponentRepository = new ComponentRepository(_dbContext);
                        LevelRepository = new LevelRepository(_dbContext);
                        PackageRepository = new PackageRepository(_dbContext);
                        PackageLabRepository = new PackageLabRepository(_dbContext);
                }
        }
}
