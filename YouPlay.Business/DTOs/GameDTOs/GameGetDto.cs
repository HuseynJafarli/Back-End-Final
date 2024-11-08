using YouPlay.Business.DTOs.CommentDTOs;
using YouPlay.Business.DTOs.GameImageDTOs;

namespace YouPlay.Business.DTOs.GameDTOs
{
    public record GameGetDto(
       int Id,
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
       List<GameImageGetDto> GameImages,
       //List<CommentGetDto> Comments,
       DateTime CreatedDate,
       DateTime? ModifiedDate,
       bool IsDeleted
   );
}
