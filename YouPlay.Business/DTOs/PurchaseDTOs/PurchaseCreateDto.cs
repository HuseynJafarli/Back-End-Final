using FluentValidation;
using YouPlay.Business.DTOs.PurchaseItemDTOs;

namespace YouPlay.Business.DTOs.PurchaseDTOs
{
    public record PurchaseCreateDto(
        string Fullname,
        string Country,
        string City,
        string EmailAddress,
        string Phone,
        string Address,
        string ZipCode,
        string Note,
        string UserId,
        decimal TotalPrice,
        List<PurchaseItemCreateDto> PurchaseItems
    );

    public class PurchaseCreateDtoValidator : AbstractValidator<PurchaseCreateDto>
    {
        public PurchaseCreateDtoValidator()
        {
            RuleFor(p => p.Fullname)
                .NotEmpty()
                .WithMessage("Fullname is required.")
                .MaximumLength(100)
                .WithMessage("Fullname must not exceed 100 characters.");

            RuleFor(p => p.Country)
                .NotEmpty()
                .WithMessage("Country is required.")
                .MaximumLength(50)
                .WithMessage("Country must not exceed 50 characters.");

            RuleFor(p => p.City)
                .NotEmpty()
                .WithMessage("City is required.")
                .MaximumLength(50)
                .WithMessage("City must not exceed 50 characters.");

            RuleFor(p => p.EmailAddress)
                .NotEmpty()
                .WithMessage("Email Address is required.")
                .EmailAddress()
                .WithMessage("Email Address is not valid.")
                .MaximumLength(100)
                .WithMessage("Email Address must not exceed 100 characters.");

            RuleFor(p => p.Address)
                .NotEmpty()
                .WithMessage("Address is required.")
                .MaximumLength(200)
                .WithMessage("Address must not exceed 200 characters.");

            RuleFor(p => p.ZipCode)
                .NotEmpty()
                .WithMessage("Zip Code is required.")
                .MaximumLength(20)
                .WithMessage("Zip Code must not exceed 20 characters.");

            RuleFor(p => p.Phone)
                .NotEmpty()
                .WithMessage("Phone is required.")
                .MaximumLength(15)
                .WithMessage("Phone must not exceed 15 characters.");

            RuleFor(p => p.TotalPrice)
                .GreaterThan(0)
                .WithMessage("Total Price must be greater than zero.");
        }
    }
}
