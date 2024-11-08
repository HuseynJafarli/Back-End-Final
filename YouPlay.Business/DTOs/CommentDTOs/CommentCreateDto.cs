using FluentValidation;

namespace YouPlay.Business.DTOs.CommentDTOs
{
    public record CommentCreateDto(
        string UserId,
        int GameId,
        string Content,
        bool IsPositive
    );

    public class CommentCreateDtoValidator : AbstractValidator<CommentCreateDto>
    {
        public CommentCreateDtoValidator()
        {
            RuleFor(c => c.Content)
                .NotEmpty()
                .WithMessage("Content is required.")
                .MaximumLength(500)
                .WithMessage("Content must not exceed 500 characters.");

            RuleFor(c => c.UserId)
                .NotEmpty()
                .WithMessage("UserId is required.");

            RuleFor(c => c.GameId)
                .GreaterThan(0)
                .WithMessage("GameId must be greater than zero.");
        }
    }
}
