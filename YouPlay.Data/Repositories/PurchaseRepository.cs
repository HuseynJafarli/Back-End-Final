using YouPlay.Core.Repositories;
using YouPlay.Data.Contexts;

namespace YouPlay.Data.Repositories
{
    public class PurchaseRepository : GenericRepository<Purchase>, IPurchaseRepository
    {
        public PurchaseRepository(AppDbContext context) : base(context)
        {
        }
    }
}
