namespace YouPlay.Core.Entities
{
    public class Game : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Rating { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }
        public int Discount { get; set; }
        public string Genre { get; set; }
        public string Developer { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string TrailerUrl { get; set; } 

        //navigations

        public List<Comment> Comments { get; set; }
        public List<GameImage> GameImages { get; set; } 

    }

}
