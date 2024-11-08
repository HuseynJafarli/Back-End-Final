using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace YouPlay.Business.DTOs.GameDTOs
{
    public record GameUpdateDto(
        string Title,
        string Description,
        decimal Rating,
        decimal CostPrice,
        decimal SalePrice,
        int Discount,
        string Genre,
        DateTime ReleaseDate,
        string TrailerUrl,
        string Developer,
        IEnumerable<IFormFile>? GameImages
    );

    public class GameUpdateDtoValidator : AbstractValidator<GameUpdateDto>
        {
            public GameUpdateDtoValidator()
            {
                RuleFor(x => x.Title)
                    .NotEmpty()
                    .WithMessage("Title is required.")
                    .MaximumLength(100);

                RuleFor(x => x.Description)
                    .NotEmpty()
                    .WithMessage("Description is required.")
                    .MaximumLength(1000);

                RuleFor(x => x.CostPrice)
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("Cost price must be at least 0.");

                RuleFor(x => x.SalePrice)
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("Sale price must be at least 0.");

                RuleFor(x => x.Discount)
                    .InclusiveBetween(0, 100)
                    .WithMessage("Discount must be between 0% and 100%.");

                RuleFor(x => x.Genre)
                    .NotEmpty()
                    .WithMessage("Genre is required.");

                RuleFor(x => x.ReleaseDate)
                    .NotEmpty()
                    .WithMessage("Release date is required.");

                RuleFor(x => x.TrailerUrl)
                    .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
                    .WithMessage("Trailer URL must be a valid URL.");

                RuleFor(x => x.Developer)
                    .NotEmpty()
                    .WithMessage("Developer is required.");

                RuleFor(x => x.NewImages)
                    .NotNull()
                    .WithMessage("New images collection should not be null.")
                    .When(x => x.NewImages != null)
                    .WithMessage("New images collection cannot be empty if provided.");

            }
        }

}
