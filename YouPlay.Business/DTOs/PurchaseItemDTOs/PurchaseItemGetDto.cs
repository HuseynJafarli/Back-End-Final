using YouPlay.Business.DTOs.GameDTOs;

namespace YouPlay.Business.DTOs.PurchaseItemDTOs
{
    public record PurchaseItemGetDto(
        int Id,
        int PurchaseId,
        int GameId,
        DateTime CreatedDate,
        DateTime? ModifiedDate,
        bool IsDeleted
    );
}
