using kit_stem_api.Data;
using kit_stem_api.Models.Domain;
using kit_stem_api.Repositories;

namespace kit_stem_api.Repositories
{
    public class UnitOfWork
    {
        private readonly KitStemDbContext _dbContext;
        public LabRepository LabRepository { get; }
        public CategoryRepository CategoryRepository { get; }
        public ComponentTypeRepository ComponentTypeRepository { get; }
        public ComponentRepository ComponentRepository { get; }
<<<<<<< HEAD
        public LevelRepository LevelRepository { get; }
=======
        public PackageRepository PackageRepository { get; }

>>>>>>> 53b4f055d3ec332a7459d4aee0bc8b3a2e9f5be5
        public UnitOfWork(KitStemDbContext dbContext)
        {
            _dbContext = dbContext;
            LabRepository = new LabRepository(_dbContext);
            CategoryRepository = new CategoryRepository(_dbContext);
            ComponentTypeRepository = new ComponentTypeRepository(_dbContext);
            ComponentRepository = new ComponentRepository(_dbContext);
<<<<<<< HEAD
            LevelRepository = new LevelRepository(_dbContext) { };

=======
            PackageRepository = new PackageRepository(_dbContext);
>>>>>>> 53b4f055d3ec332a7459d4aee0bc8b3a2e9f5be5
        }



    }
}
