using Microsoft.AspNetCore.Identity;

namespace YouPlay.Core.Entities
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public List<Purchase> Purchases { get; set; } 
        public List<Comment> Comments { get; set; }
        public List<Friendship> Friendships { get; set; }
        public string ProfileImageUrl { get; set; }

    }
}
