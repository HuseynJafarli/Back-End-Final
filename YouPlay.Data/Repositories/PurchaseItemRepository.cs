using YouPlay.Core.Entities;
using YouPlay.Core.Repositories;
using YouPlay.Data.Contexts;

namespace YouPlay.Data.Repositories
{
    public class PurchaseItemRepository : GenericRepository<PurchaseItem>, IPurchaseItemRepository
    {
        public PurchaseItemRepository(AppDbContext context) : base(context)
        {
        }
    }
}
