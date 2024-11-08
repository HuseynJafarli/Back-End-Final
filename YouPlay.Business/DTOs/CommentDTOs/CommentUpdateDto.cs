namespace YouPlay.Business.DTOs.CommentDTOs
{
    public record CommentUpdateDto(
        string Content,
        bool IsPositive
    );
}
