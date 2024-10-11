using KST.Api.Data;
using KST.Api.Models.Domain;

namespace KST.Api.Repositories
{
    public class LabSupportRepository : GenericRepository<LabSupport>
    {
        public LabSupportRepository(KitStemDbContext context) : base(context) { }
    }
}
