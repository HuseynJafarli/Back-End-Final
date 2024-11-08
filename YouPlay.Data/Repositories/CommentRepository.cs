using YouPlay.Core.Entities;
using YouPlay.Core.Repositories;
using YouPlay.Data.Contexts;

namespace YouPlay.Data.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(AppDbContext context) : base(context)
        {
        }
    }
}
