using YouPlay.Core.Entities;
using YouPlay.Core.Repositories;
using YouPlay.Data.Contexts;

namespace YouPlay.Data.Repositories
{
    public class GameRepository : GenericRepository<Game>, IGameRepository
    {
        public GameRepository(AppDbContext context) : base(context)
        {
        }
    }
}
