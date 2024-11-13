using System.ComponentModel.DataAnnotations.Schema;

namespace YouPlay.Core.Entities
{
    public class Game : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Rating { get; set; }
        public decimal CostPrice { get; set; }
        public int Discount { get; set; }

        public string Genre { get; set; }
        public string Developer { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string TrailerUrl { get; set; } 

        [NotMapped]
        public decimal SalePrice => Math.Round(CostPrice * (decimal)(1 - (Discount / 100.0)), 0);
        //navigations

        public List<Comment> Comments { get; set; }
        public List<GameImage> GameImages { get; set; } 

    }

}
