using YouPlay.Core.Entities;

public class Purchase : BaseEntity
{
    public string Fullname { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string EmailAddress { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string ZipCode { get; set; }
    public string Note { get; set; }

    public string UserId { get; set; }
    public decimal TotalPrice { get; set; }

    //navigation
    public AppUser User { get; set; }
    public List<PurchaseItem> PurchaseItems { get; set; } // PurchaseItems instead of Game
}
