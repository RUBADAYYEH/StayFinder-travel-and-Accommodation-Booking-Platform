namespace Domain.Entities
{
    public class Payment
    {
        public Guid PaymentId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "Euro";
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; }
    }

}
