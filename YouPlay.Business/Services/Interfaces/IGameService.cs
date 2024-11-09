using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using YouPlay.Business.DTOs.GameDTOs;
using YouPlay.Core.Entities;

namespace YouPlay.Business.Services.Interfaces
{
    public interface IGameService
    {
        Task<GameGetDto> CreateAsync(GameCreateDto dto, IFormFileCollection imageFiles);
        Task<GameGetDto> UpdateAsync(int id, GameUpdateDto dto, IFormFileCollection newImageFiles = null);
        Task DeleteAsync(int id);
        Task<ICollection<GameGetDto>> GetByExpessionAsync(bool AsNoTracking = false, Expression<Func<Game, bool>>? expression = null, params string[] includes);
        Task<GameGetDto> GetByIdAsync(int id);
        Task<GameGetDto> GetOneByExpressionAsync(bool AsNoTracking = false, Expression<Func<Game, bool>>? expression = null, params string[] includes);
    }
}
