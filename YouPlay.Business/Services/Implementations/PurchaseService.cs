using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using YouPlay.Business.DTOs.PurchaseDTOs;
using YouPlay.Business.DTOs.PurchaseItemDTOs;
using YouPlay.Business.Exceptions.Common;
using YouPlay.Business.Services.Interfaces;
using YouPlay.Core.Entities;
using YouPlay.Core.Repositories;
using YouPlay.Data.Repositories;

namespace YouPlay.Business.Services.Implementations
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository purchaseRepository;
        private readonly IGameRepository gameRepository;

        public PurchaseService(IPurchaseRepository purchaseRepository, IGameRepository gameRepository)
        {
            this.purchaseRepository = purchaseRepository;
            this.gameRepository = gameRepository;
        }

        public async Task<decimal> CalculateTotalPrice(ICollection<PurchaseItemCreateDto> purchaseItems)
        {
            var gameIds = purchaseItems.Select(item => item.GameId).ToList();

            var games = await gameRepository.GetByIdsAsync(gameIds);

            decimal totalPrice = 0;

            foreach (var item in purchaseItems)
            {
                var game = games.FirstOrDefault(g => g.Id == item.GameId);
                if (game != null)
                {
                    var discountedPrice = game.SalePrice * (1 - (game.Discount / 100m));
                    totalPrice += discountedPrice;
                }
            }

            return totalPrice;
        }


        public async Task<PurchaseGetDto> CreateAsync(PurchaseCreateDto dto)
        {
            var totalPrice = await CalculateTotalPrice(dto.PurchaseItems);

            var purchase = new Purchase
            {
                Fullname = dto.Fullname,
                Country = dto.Country,
                City = dto.City,
                EmailAddress = dto.EmailAddress,
                Phone = dto.Phone,
                Address = dto.Address,
                ZipCode = dto.ZipCode,
                Note = dto.Note,
                TotalPrice = totalPrice, 
                PurchaseItems = dto.PurchaseItems.Select(item => new PurchaseItem
                {
                    GameId = item.GameId

                }).ToList()
            };

            await purchaseRepository.CreateAsync(purchase);
            await purchaseRepository.CommitAsync();

            var purchaseDto = new PurchaseGetDto(
                Id: purchase.Id,
                Fullname: purchase.Fullname,
                Country: purchase.Country,
                City: purchase.City,
                EmailAddress: purchase.EmailAddress,
                TotalPrice: purchase.TotalPrice,
                Phone: purchase.Phone,
                Address: purchase.Address,
                ZipCode: purchase.ZipCode,
                Note: purchase.Note,
                UserId: purchase.UserId,
                PurchaseItems: new List<PurchaseItemGetDto>(),
                CreatedDate: DateTime.Now,
                ModifiedDate: DateTime.Now,
                IsDeleted: false

            );


            return purchaseDto; 
        }


        public async Task DeleteAsync(int id)
        {
            var purchase = await purchaseRepository.GetByIdAsync(id);
            if (purchase == null) throw new EntityNotFoundException("Purchase not found!");

            purchaseRepository.Delete(purchase);
            await purchaseRepository.CommitAsync();
        }

        public async Task<ICollection<PurchaseGetDto>> GetByExpessionAsync(bool AsNoTracking = false, Expression<Func<Purchase, bool>>? expression = null, params string[] includes)
        {
            var purchases = await purchaseRepository.GetByExpression(AsNoTracking, expression, includes).ToListAsync();
            if (purchases == null || purchases.Count == 0) throw new EntityNotFoundException("Purchases not found!");
            return purchases.Select(p => new PurchaseGetDto(
                Id: p.Id,
                Fullname: p.Fullname,
                Country: p.Country,
                City: p.City,
                EmailAddress: p.EmailAddress,
                TotalPrice: p.TotalPrice,
                Phone: p.Phone,
                Address: p.Address,
                ZipCode: p.ZipCode,
                Note: p.Note,
                UserId: p.UserId,
                PurchaseItems: p.PurchaseItems.Select(item => new PurchaseItemGetDto(
                    Id: item.Id,
                    PurchaseId: item.PurchaseId,
                    GameId: item.GameId,
                    CreatedDate: item.CreatedDate,
                    ModifiedDate: item.ModifiedDate,
                    IsDeleted: item.IsDeleted
                )).ToList(),
                CreatedDate: p.CreatedDate,
                ModifiedDate: p.ModifiedDate,
                IsDeleted: p.IsDeleted
            )).ToList();
        }

        public async Task<PurchaseGetDto> GetByIdAsync(int id)
        {
            var purchase = await purchaseRepository.GetByIdAsync(id);
            if (purchase == null) throw new EntityNotFoundException("Purchase not found!");

            return new PurchaseGetDto(
                Id: purchase.Id,
                Fullname: purchase.Fullname,
                Country: purchase.Country,
                City: purchase.City,
                EmailAddress: purchase.EmailAddress,
                TotalPrice: purchase.TotalPrice,
                Phone: purchase.Phone,
                Address: purchase.Address,
                ZipCode: purchase.ZipCode,
                Note: purchase.Note,
                UserId: purchase.UserId,
                PurchaseItems: purchase.PurchaseItems.Select(item => new PurchaseItemGetDto(
                    Id: item.Id,
                    PurchaseId: item.PurchaseId,
                    GameId: item.GameId,
                    CreatedDate: item.CreatedDate,
                    ModifiedDate: item.ModifiedDate,
                    IsDeleted: item.IsDeleted
                )).ToList(),
                CreatedDate: purchase.CreatedDate,
                ModifiedDate: purchase.ModifiedDate,
                IsDeleted: purchase.IsDeleted
            );
        }

        public async Task<PurchaseGetDto> GetOneByExpressionAsync(bool AsNoTracking = false, Expression<Func<Purchase, bool>>? expression = null, params string[] includes)
        {
            var query = purchaseRepository.GetByExpression(AsNoTracking, expression, includes);

            var purchase = await query.FirstOrDefaultAsync();
            if (purchase == null) throw new EntityNotFoundException("Purchase not found!");

            var purchaseDto = new PurchaseGetDto(
                Id: purchase.Id,
                Fullname: purchase.Fullname,
                Country: purchase.Country,
                City: purchase.City,
                EmailAddress: purchase.EmailAddress,
                TotalPrice: purchase.TotalPrice,
                Phone: purchase.Phone,
                Address: purchase.Address,
                ZipCode: purchase.ZipCode,
                Note: purchase.Note,
                UserId: purchase.UserId,
                PurchaseItems: purchase.PurchaseItems.Select(item => new PurchaseItemGetDto(
                    Id: item.Id,
                    PurchaseId: item.PurchaseId,
                    GameId: item.GameId,
                    CreatedDate: item.CreatedDate,
                    ModifiedDate: item.ModifiedDate,
                    IsDeleted: item.IsDeleted
                )).ToList(),
                CreatedDate: purchase.CreatedDate,
                ModifiedDate: purchase.ModifiedDate,
                IsDeleted: purchase.IsDeleted
            );

            return purchaseDto;
        }


        public Task UpdateAsync(int id, PurchaseUpdateDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
