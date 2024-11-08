using System.Linq.Expressions;
using YouPlay.Business.DTOs.CommentDTOs;
using YouPlay.Business.DTOs.GameDTOs;
using YouPlay.Core.Entities;

namespace YouPlay.Business.Services.Interfaces
{
    public interface ICommentService
    {
        Task CreateAsync(CommentCreateDto dto);
        Task UpdateAsync(int id, CommentUpdateDto dto);
        Task DeleteAsync(int id);
        Task<ICollection<CommentGetDto>> GetByExpessionAsync(bool AsNoTracking = false, Expression<Func<Game, bool>>? expression = null, params string[] includes);
        Task<CommentGetDto> GetByIdAsync(int id);
        Task<CommentGetDto> GetOneByExpressionAsync(bool AsNoTracking = false, Expression<Func<Game, bool>>? expression = null, params string[] includes);
    }
}
