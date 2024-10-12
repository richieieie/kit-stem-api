using KSH.Api.Data;
using KSH.Api.Models.Domain;
using KSH.Api.Repositories;

namespace KST.Api.Repositories
{
    public class LabSupportRepository : GenericRepository<LabSupport>
    {
        public LabSupportRepository(KitStemDbContext context) : base(context) { }
    }
}
