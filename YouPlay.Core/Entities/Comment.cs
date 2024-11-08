namespace YouPlay.Core.Entities
{
    public class Comment: BaseEntity
    {
        public string UserId { get; set; }
        public int GameId { get; set; }
        public string Content { get; set; }
        public bool IsPositive { get; set; }

        // Navigation Properties
        public AppUser User { get; set; } 
        public Game Game { get; set; } 

    }
}
