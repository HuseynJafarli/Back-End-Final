namespace YouPlay.MVC.ViewModels
{
    public record GameGetVM(
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
       List<GameImageGetVM> GameImages,
       //List<CommentGetVM> Comments,
       DateTime CreatedDate,
       DateTime? ModifiedDate,
       bool IsDeleted
   );
}
