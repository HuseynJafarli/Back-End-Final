using YouPlay.Business.DTOs.PurchaseItemDTOs;

namespace YouPlay.Business.DTOs.PurchaseDTOs
{
    public record PurchaseGetDto(
       int Id,
       string Fullname,
       string Country,
       string City,
       string EmailAddress,
       decimal TotalPrice,
       string Phone,
       string Address,
       string ZipCode,
       string Note,
       string UserId,
       List<PurchaseItemGetDto> PurchaseItems,
       DateTime CreatedDate,
       DateTime? ModifiedDate,
       bool IsDeleted
   );
}
