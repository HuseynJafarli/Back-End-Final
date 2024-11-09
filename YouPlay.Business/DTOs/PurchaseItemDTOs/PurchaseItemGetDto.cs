using YouPlay.Business.DTOs.GameDTOs;

namespace YouPlay.Business.DTOs.PurchaseItemDTOs
{
    public record PurchaseItemGetDto(
        int PurchaseId,
        int GameId,
        DateTime CreatedDate
    );
}
