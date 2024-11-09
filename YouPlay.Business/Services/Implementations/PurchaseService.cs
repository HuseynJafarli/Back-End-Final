using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using YouPlay.Business.DTOs.PurchaseDTOs;
using YouPlay.Business.DTOs.PurchaseItemDTOs;
using YouPlay.Business.Exceptions.Common;
using YouPlay.Business.Services.Interfaces;
using YouPlay.Core.Entities;
using YouPlay.Core.Repositories;

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

        public async Task<decimal> CalculateTotalPriceAsync(ICollection<PurchaseItemCreateDto> purchaseItems)
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
            var totalPrice = await CalculateTotalPriceAsync(dto.PurchaseItems);

            var purchaseItems = dto.PurchaseItems.Select(item => new PurchaseItem
            {
                GameId = item.GameId,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            }).ToList();

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
                UserId = dto.UserId,
                TotalPrice = totalPrice,
                PurchaseItems = purchaseItems,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };

            await purchaseRepository.CreateAsync(purchase);
            await purchaseRepository.CommitAsync();

            var purchaseDto = new PurchaseGetDto
            (
                Id: purchase.Id,
                Fullname: purchase.Fullname,
                Country: purchase.Country,
                City: purchase.City,
                EmailAddress: purchase.EmailAddress,
                Phone: purchase.Phone,
                Address: purchase.Address,
                ZipCode: purchase.ZipCode,
                Note: purchase.Note,
                UserId: purchase.UserId,
                TotalPrice: purchase.TotalPrice,
                PurchaseItems: purchase.PurchaseItems.Select(item => new PurchaseItemGetDto(
                    PurchaseId: item.PurchaseId,
                    GameId: item.GameId,
                    CreatedDate: item.CreatedDate
                )).ToList(),
                CreatedDate: purchase.CreatedDate,
                ModifiedDate: purchase.ModifiedDate,
                IsDeleted: purchase.IsDeleted
            );

            return purchaseDto;
        }
        //public async Task DeleteAsync(int id)
        //{
        //    if (id < 1) throw new InvalidIdException("Invalid Purchase Id.");

        //    var purchase = await purchaseRepository.GetByExpression(true, p => p.Id == id, "PurchaseItems").FirstOrDefaultAsync();
        //    if (purchase == null) throw new EntityNotFoundException("Purchase not found!");

        //    // Delete all associated purchase items
        //    foreach (var item in purchase.PurchaseItems.ToList())
        //    {
        //        purchaseRepository.Delete(item);
        //    }

        //    // Delete the purchase
        //    purchaseRepository.Delete(purchase);
        //    await purchaseRepository.CommitAsync();
        //}

        //public async Task<ICollection<PurchaseGetDto>> GetByExpressionAsync(bool AsNoTracking = false, Expression<Func<Purchase, bool>>? expression = null, params string[] includes)
        //{
        //    var purchases = await purchaseRepository.GetByExpression(AsNoTracking, expression, includes).ToListAsync();
        //    if (purchases == null || !purchases.Any()) throw new EntityNotFoundException("No purchases found.");

        //    // Map to DTOs
        //    return purchases.Select(purchase => new PurchaseGetDto(
        //        Id: purchase.Id,
        //        Fullname: purchase.Fullname,
        //        Country: purchase.Country,
        //        City: purchase.City,
        //        EmailAddress: purchase.EmailAddress,
        //        Phone: purchase.Phone,
        //        Address: purchase.Address,
        //        ZipCode: purchase.ZipCode,
        //        Note: purchase.Note,
        //        UserId: purchase.UserId,
        //        TotalPrice: purchase.TotalPrice,
        //        PurchaseItems: purchase.PurchaseItems.Select(item => new PurchaseItemGetDto(
        //            Id: item.Id,
        //            PurchaseId: item.PurchaseId,
        //            GameId: item.GameId,
        //            CreatedDate: item.CreatedDate,
        //            ModifiedDate: item.ModifiedDate,
        //            IsDeleted: item.IsDeleted
        //        )).ToList(),
        //        CreatedDate: purchase.CreatedDate,
        //        ModifiedDate: purchase.ModifiedDate,
        //        IsDeleted: purchase.IsDeleted
        //    )).ToList();
        //}

        //public async Task<PurchaseGetDto> GetByIdAsync(int id)
        //{
        //    if (id < 1) throw new InvalidIdException("Invalid Purchase Id.");

        //    var purchase = await purchaseRepository.GetByExpression(true, p => p.Id == id, "PurchaseItems").FirstOrDefaultAsync();
        //    if (purchase == null) throw new EntityNotFoundException("Purchase not found!");

        //    // Map to DTO
        //    var purchaseDto = new PurchaseGetDto(
        //       Id: purchase.Id,
        //        Fullname: purchase.Fullname,
        //        Country: purchase.Country,
        //        City: purchase.City,
        //        EmailAddress: purchase.EmailAddress,
        //        Phone: purchase.Phone,
        //        Address: purchase.Address,
        //        ZipCode: purchase.ZipCode,
        //        Note: purchase.Note,
        //        UserId: purchase.UserId,
        //        TotalPrice: purchase.TotalPrice,
        //        PurchaseItems: purchase.PurchaseItems.Select(item => new PurchaseItemGetDto(
        //            Id: item.Id,
        //            PurchaseId: item.PurchaseId,
        //            GameId: item.GameId,
        //            CreatedDate: item.CreatedDate,
        //            ModifiedDate: item.ModifiedDate,
        //            IsDeleted: item.IsDeleted
        //        )).ToList(),
        //        CreatedDate: purchase.CreatedDate,
        //        ModifiedDate: purchase.ModifiedDate,
        //        IsDeleted: purchase.IsDeleted
        //    );

        //    return purchaseDto;
        //}

        //public Task<PurchaseGetDto> GetOneByExpressionAsync(bool AsNoTracking = false, Expression<Func<Purchase, bool>>? expression = null, params string[] includes)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task UpdateAsync(int id, PurchaseUpdateDto dto)
        //{
        //    throw new NotImplementedException();
        //}

    }
}
