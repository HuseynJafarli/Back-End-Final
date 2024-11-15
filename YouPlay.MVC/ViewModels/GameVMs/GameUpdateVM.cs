using System.ComponentModel.DataAnnotations;

namespace YouPlay.MVC.ViewModels
{
    public class GameUpdateVM
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Range(0, 10)]
        public decimal Rating { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal CostPrice { get; set; }

        public int Discount { get; set; }
        public string Genre { get; set; }
        public string Developer { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        public string TrailerUrl { get; set; }

        public IEnumerable<IFormFile>? GameImages { get; set; }
    }
}
