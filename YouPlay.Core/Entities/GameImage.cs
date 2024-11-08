namespace YouPlay.Core.Entities
{
    public class GameImage: BaseEntity
    {
        public int GameId { get; set; } // Foreign key to the Game
        public string ImageUrl { get; set; } // URL of the image
        public Game Game { get; set; }

    }
}
