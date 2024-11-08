namespace YouPlay.Business.DTOs.PurchaseDTOs
{
    public record PurchaseUpdateDto(
        string Fullname,
        string Country,
        string City,
        string Address,
        string ZipCode,
        string Note
    );
}
