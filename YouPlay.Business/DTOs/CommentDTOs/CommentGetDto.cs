namespace YouPlay.Business.DTOs.CommentDTOs
{
    public record CommentGetDto(
        int Id,
        string Content,
        bool IsPositive,
        string UserName,
        DateTime CreatedDate,
        DateTime? ModifiedDate,
        bool IsDeleted
    );
}
