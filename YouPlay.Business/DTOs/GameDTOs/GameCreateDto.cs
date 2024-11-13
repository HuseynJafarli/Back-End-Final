using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace YouPlay.Business.DTOs.GameDTOs
{
    public record GameCreateDto(
        string Title,
        string Description,
        decimal Rating,
        decimal CostPrice,
        int Discount,
        string Genre,
        DateTime ReleaseDate,
        string TrailerUrl,
        string Developer,
        IEnumerable<IFormFile> GameImages  
    );

    public class GameCreateDtoValidator : AbstractValidator<GameCreateDto>
    {
        public GameCreateDtoValidator()
        {
            RuleFor(g => g.Title)
                .NotEmpty()
                .WithMessage("Title is required.")
                .MaximumLength(100)
                .WithMessage("Title must not exceed 100 characters.");

            RuleFor(g => g.Description)
                .MaximumLength(1000)
                .WithMessage("Description must not exceed 1000 characters.");

            RuleFor(g => g.Discount)
                .InclusiveBetween(0, 100)
                .WithMessage("Discount must be between 0 and 100.");

            RuleFor(g => g.ReleaseDate)
                .NotEmpty()
                .WithMessage("Release Date is required.");

            RuleFor(g => g.GameImages)
                .NotNull()
                .NotEmpty()
                .WithMessage("At least one image is required.");

            RuleForEach(g => g.GameImages)
                .Must(file => file.ContentType.StartsWith("image/"))
                .WithMessage("All files must be images.")
                .Must(file => file.Length < 5*1024*1024)
                .WithMessage("Each image size must be less than 5mb.");
        }
    }
}
