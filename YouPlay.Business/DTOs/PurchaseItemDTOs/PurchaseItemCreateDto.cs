using FluentValidation;

namespace YouPlay.Business.DTOs.PurchaseItemDTOs
{
    public record PurchaseItemCreateDto(
        int GameId
    );

    public class PurchaseItemCreateDtoValidator : AbstractValidator<PurchaseItemCreateDto>
    {
        public PurchaseItemCreateDtoValidator()
        {
            RuleFor(pi => pi.GameId)
                .GreaterThan(0)
                .WithMessage("GameId must be greater than zero.");

            //RuleFor(pi => pi.PurchaseId)
            //    .GreaterThan(0)
            //    .WithMessage("PurchaseId must be greater than zero.");
        }
    }

}
