namespace YouPlay.Core.Entities
{
    public class Friendship : BaseEntity
    {
        public string UserId { get; set; } // Requesting user
        public string FriendUserId { get; set; } // Friend user

        public bool IsAccepted { get; set; } // Status of the friend request

        // Navigation Properties
        public AppUser User { get; set; } // The user who sent the friend request
        public AppUser Friend { get; set; } // The user who received the friend request
    }

}
