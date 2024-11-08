using YouPlay.Core.Entities;
using YouPlay.Core.Repositories;
using YouPlay.Data.Contexts;

namespace YouPlay.Data.Repositories
{
    public class FriendshipRepository : GenericRepository<Friendship>, IFriendShipRepository
    {
        public FriendshipRepository(AppDbContext context) : base(context)
        {
        }
    }
}
