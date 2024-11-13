using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace YouPlay.Business.DTOs.GameDTOs
{
    public record GameUpdateDto(
        string Title,
        string Description,
        decimal Rating,
        decimal CostPrice,
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

            }
        }

}
