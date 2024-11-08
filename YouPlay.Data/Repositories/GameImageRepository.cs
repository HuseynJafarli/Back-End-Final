using YouPlay.Core.Entities;
using YouPlay.Core.Repositories;
using YouPlay.Data.Contexts;

namespace YouPlay.Data.Repositories
{
    public class GameImageRepository : GenericRepository<GameImage>, IGameImageRepository
    {
        public GameImageRepository(AppDbContext context) : base(context)
        {
        }
    }
}
