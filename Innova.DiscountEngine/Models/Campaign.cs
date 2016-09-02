namespace Innova.DiscountEngine.Models
{
    public class Campaign
    {
        public int Id { get; set; } 
        public decimal StartingAmount { get; set; }
        public decimal EndingAmount { get; set; }
        public Discount Discount { get; set; }
    }
}