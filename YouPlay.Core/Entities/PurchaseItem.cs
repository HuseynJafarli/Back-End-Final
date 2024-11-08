namespace YouPlay.Core.Entities
{
    public class PurchaseItem : BaseEntity
    {
        public int PurchaseId { get; set; }
        public int GameId { get; set; }
        public Purchase Purchase { get; set; }
        public Game Game { get; set; }

    }
}
