using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using YouPlay.Business.DTOs.CommentDTOs;
using YouPlay.Business.DTOs.GameDTOs;
using YouPlay.Business.DTOs.GameImageDTOs;
using YouPlay.Business.Exceptions.Common;
using YouPlay.Business.Services.Interfaces;
using YouPlay.Business.Utilities;
using YouPlay.Core.Entities;
using YouPlay.Core.Repositories;

namespace YouPlay.Business.Services.Implementations
{
    public class GameService : IGameService
    {
        private readonly IGameRepository gameRepository;
        private readonly IGameImageRepository gameImageRepository;

        public GameService(IGameRepository gameRepository, IGameImageRepository gameImageRepository)
        {
            this.gameRepository = gameRepository;
            this.gameImageRepository = gameImageRepository;
        }

        public async Task<GameGetDto> CreateAsync(GameCreateDto dto, IFormFileCollection imageFiles)
        {
            Game game = new Game()
            {
                Title = dto.Title,
                Description = dto.Description,
                Rating = dto.Rating,
                CostPrice = dto.CostPrice,
                Genre = dto.Genre,
                Discount = dto.Discount,
                Developer = dto.Developer,
                ReleaseDate = dto.ReleaseDate,
                TrailerUrl = dto.TrailerUrl,
                Comments = new List<Comment>(),
                GameImages = new List<GameImage>(),
                IsDeleted = false,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,

            };

            foreach (var file in imageFiles)
            {
                var fileName = file.SaveFile("wwwroot", "Uploads");

                game.GameImages.Add(new GameImage
                {
                    ImageUrl = "https://localhost:7283/uploads/" + fileName,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    IsDeleted = false
                });
            }

            await gameRepository.CreateAsync(game);
            await gameRepository.CommitAsync();

            var gameDto = new GameGetDto(
                  Id: game.Id,
                  Title: game.Title,
                  Description: game.Description,
                  Rating: game.Rating,
                  CostPrice: game.CostPrice,
                  SalePrice: game.SalePrice,
                  Discount: game.Discount,
                  Genre: game.Genre,
                  ReleaseDate: game.ReleaseDate,
                  TrailerUrl: game.TrailerUrl,
                  Developer: game.Developer,
                  GameImages: game.GameImages.Select(img => new GameImageGetDto(
                      Id: img.Id,
                      ImageUrl: "https://localhost:7283/uploads/" + img.ImageUrl
                  )).ToList(),
                  //Comments: game.Comments.Select(com => new CommentGetDto(
                  //    Id: com.Id,
                  //    Content: com.Content,
                  //    UserName: com.User.UserName,
                  //    IsPositive: com.IsPositive,
                  //    CreatedDate: com.CreatedDate,
                  //    ModifiedDate: com.ModifiedDate,
                  //    IsDeleted: com.IsDeleted
                  //)).ToList(),
                  CreatedDate: game.CreatedDate,
                  ModifiedDate: game.ModifiedDate,
                  IsDeleted: game.IsDeleted
              );

            return gameDto;
        }

        public async Task DeleteAsync(int id)
        {
            if (id < 1) throw new InvalidIdException("Id is not valid");

            var game = await gameRepository.GetByExpression(true, g => g.Id == id, "GameImages").FirstOrDefaultAsync();
            if (game == null) throw new EntityNotFoundException("Game not found!");

            foreach (GameImage img in game.GameImages.ToList())
            {
                FileManager.DeleteFile("wwwroot", "Uploads", img.ImageUrl);
                gameImageRepository.Delete(img);
            }

            gameRepository.Delete(game);
            await gameImageRepository.CommitAsync();
            await gameRepository.CommitAsync();
        }

        public async Task<ICollection<GameGetDto>> GetByExpessionAsync(bool AsNoTracking = false, Expression<Func<Game, bool>>? expression = null, params string[] includes)
        {
            var query = gameRepository.GetByExpression(AsNoTracking, expression, includes);
            var games = await query.ToListAsync();
            if (games == null || games.Count == 0) throw new EntityNotFoundException("Games not found!");

            ICollection<GameGetDto> gameDtos = games.Select(game => new GameGetDto(
                Id: game.Id,
                Title: game.Title,
                Description: game.Description,
                Rating: game.Rating,
                CostPrice: game.CostPrice,
                SalePrice: game.SalePrice,
                Discount: game.Discount,
                Genre: game.Genre,
                ReleaseDate: game.ReleaseDate,
                TrailerUrl: game.TrailerUrl,
                Developer: game.Developer,
                GameImages: game.GameImages.Select(img => new GameImageGetDto(
                    Id: img.Id,
                    ImageUrl: img.ImageUrl
                )).ToList(),
                //Comments: game.Comments.Select(com => new CommentGetDto(
                //    Id: com.Id,
                //    Content: com.Content,
                //    UserName: com.User.UserName,
                //    IsPositive: com.IsPositive,
                //    CreatedDate: com.CreatedDate,
                //    ModifiedDate: com.ModifiedDate,
                //    IsDeleted: com.IsDeleted
                //)).ToList(),
                CreatedDate: game.CreatedDate,
                ModifiedDate: game.ModifiedDate,
                IsDeleted: game.IsDeleted
            )).ToList();
            return gameDtos;
        }

        public async Task<GameGetDto> GetByIdAsync(int id)
        {
            if (id < 1) throw new InvalidIdException("Id is not valid");

            var game = await gameRepository.GetByExpression(true, g => g.Id == id, "GameImages").FirstOrDefaultAsync();

            if (game == null) throw new EntityNotFoundException("Game not found!");

            var gameDto = new GameGetDto(
                  Id: game.Id,
                  Title: game.Title,
                  Description: game.Description,
                  Rating: game.Rating,
                  CostPrice: game.CostPrice,
                  SalePrice: game.SalePrice,
                  Discount: game.Discount,
                  Genre: game.Genre,
                  ReleaseDate: game.ReleaseDate,
                  TrailerUrl: game.TrailerUrl,
                  Developer: game.Developer,
                  GameImages: game.GameImages.Select(img => new GameImageGetDto(
                      Id: img.Id,
                      ImageUrl: img.ImageUrl
                  )).ToList(),
                  //Comments: game.Comments.Select(com => new CommentGetDto(
                  //    Id: com.Id,
                  //    Content: com.Content,
                  //    UserName: com.User.UserName,
                  //    IsPositive: com.IsPositive,
                  //    CreatedDate: com.CreatedDate,
                  //    ModifiedDate: com.ModifiedDate,
                  //    IsDeleted: com.IsDeleted
                  //)).ToList(),
                  CreatedDate: game.CreatedDate,
                  ModifiedDate: game.ModifiedDate,
                  IsDeleted: game.IsDeleted
              );

            return gameDto;
        }

        public async Task<GameGetDto> GetOneByExpressionAsync(bool AsNoTracking = false, Expression<Func<Game, bool>>? expression = null, params string[] includes)
        {
            Game? game = await gameRepository.GetByExpression(AsNoTracking, expression, includes).FirstOrDefaultAsync();
            if (game == null) throw new EntityNotFoundException("Game not found!");

            var gameDto = new GameGetDto(
                  Id: game.Id,
                  Title: game.Title,
                  Description: game.Description,
                  Rating: game.Rating,
                  CostPrice: game.CostPrice,
                  SalePrice: game.SalePrice,
                  Discount: game.Discount,
                  Genre: game.Genre,
                  ReleaseDate: game.ReleaseDate,
                  TrailerUrl: game.TrailerUrl,
                  Developer: game.Developer,
                  GameImages: game.GameImages.Select(img => new GameImageGetDto(
                      Id: img.Id,
                      ImageUrl: img.ImageUrl
                  )).ToList(),
                  //Comments: game.Comments.Select(com => new CommentGetDto(
                  //    Id: com.Id,
                  //    Content: com.Content,
                  //    UserName: com.User.UserName,
                  //    IsPositive: com.IsPositive,
                  //    CreatedDate: com.CreatedDate,
                  //    ModifiedDate: com.ModifiedDate,
                  //    IsDeleted: com.IsDeleted
                  //)).ToList(),
                  CreatedDate: game.CreatedDate,
                  ModifiedDate: game.ModifiedDate,
                  IsDeleted: game.IsDeleted
              );

            return gameDto;
        }

        public async Task<GameGetDto> UpdateAsync(int id, GameUpdateDto dto, IFormFileCollection? newImageFiles = null)
        {
            if (id < 1) throw new InvalidIdException("Id is not valid");

            // Retrieve the game with its images from the repository
            var game = await gameRepository.GetByExpression(false, g => g.Id == id, "GameImages").FirstOrDefaultAsync();
            if (game == null) throw new EntityNotFoundException("Game not found!");

            // Update basic properties
            game.Title = dto.Title;
            game.Description = dto.Description;
            game.Rating = dto.Rating;
            game.CostPrice = dto.CostPrice;
            game.Discount = dto.Discount;
            game.ReleaseDate = dto.ReleaseDate;
            game.Genre = dto.Genre;
            game.TrailerUrl = dto.TrailerUrl;
            game.Developer = dto.Developer;
            game.ModifiedDate = DateTime.Now;

            // Handle image updates
            if (newImageFiles != null)
            {
                if (dto.GameImages != null)
                {
                    foreach (var img in game.GameImages.ToList())
                    {
                        FileManager.DeleteFile("wwwroot", "Uploads", img.ImageUrl);
                        gameImageRepository.Delete(img);
                    }
                }

                foreach (var newFile in newImageFiles)
                {
                    var fileName = newFile.SaveFile("wwwroot", "Uploads");

                    game.GameImages.Add(new GameImage
                    {
                        ImageUrl = "https://localhost:7283/uploads/" + fileName,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        IsDeleted = false
                    });
                }
            }

            await gameRepository.CommitAsync();
            await gameImageRepository.CommitAsync();


            var gameDto = new GameGetDto(
                  Id: game.Id,
                  Title: game.Title,
                  Description: game.Description,
                  Rating: game.Rating,
                  CostPrice: game.CostPrice,
                  SalePrice: game.SalePrice,
                  Discount: game.Discount,
                  Genre: game.Genre,
                  ReleaseDate: game.ReleaseDate,
                  TrailerUrl: game.TrailerUrl,
                  Developer: game.Developer,
                  GameImages: game.GameImages.Select(img => new GameImageGetDto(
                      Id: img.Id,
                      ImageUrl: img.ImageUrl
                  )).ToList(),
                  //Comments: game.Comments.Select(com => new CommentGetDto(
                  //    Id: com.Id,
                  //    Content: com.Content,
                  //    UserName: com.User.UserName,
                  //    IsPositive: com.IsPositive,
                  //    CreatedDate: com.CreatedDate,
                  //    ModifiedDate: com.ModifiedDate,
                  //    IsDeleted: com.IsDeleted
                  //)).ToList(),
                  CreatedDate: game.CreatedDate,
                  ModifiedDate: game.ModifiedDate,
                  IsDeleted: game.IsDeleted
              );

            return gameDto;

        }


    }
}
