using System.Linq.Expressions;
using YouPlay.Business.DTOs.GameDTOs;
using YouPlay.Business.DTOs.PurchaseDTOs;
using YouPlay.Business.DTOs.PurchaseItemDTOs;
using YouPlay.Core.Entities;

namespace YouPlay.Business.Services.Interfaces
{
    public interface IPurchaseService
    {
        Task<PurchaseGetDto> CreateAsync(PurchaseCreateDto dto);
        Task UpdateAsync(int id, PurchaseUpdateDto dto);
        Task DeleteAsync(int id);
        Task<ICollection<PurchaseGetDto>> GetByExpessionAsync(bool AsNoTracking = false, Expression<Func<Purchase, bool>>? expression = null, params string[] includes);
        Task<PurchaseGetDto> GetByIdAsync(int id);
        Task<PurchaseGetDto> GetOneByExpressionAsync(bool AsNoTracking = false, Expression<Func<Purchase, bool>>? expression = null, params string[] includes);
        Task<decimal> CalculateTotalPriceAsync(ICollection<PurchaseItemCreateDto> purchaseItems);
        Task<ICollection<PurchaseGetDto>> GetByExpressionAsync();
    }
}
