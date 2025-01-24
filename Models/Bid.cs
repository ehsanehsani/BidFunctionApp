namespace BidFunctionApp.Models
{
    public class Bid
    {
        public int Id { get; set; }
        public string BidderName { get; set; }
        public decimal Amount { get; set; }
        public DateTime BidTime { get; set; }
    }
}
