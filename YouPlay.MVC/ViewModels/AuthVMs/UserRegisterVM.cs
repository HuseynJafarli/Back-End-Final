using System.ComponentModel.DataAnnotations;

namespace YouPlay.MVC.ViewModels
{
    public class UserRegisterVM
    {

        [Required]
        public string Fullname { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
        public IFormFile? ProfileImage { get; set; }
    }
}
