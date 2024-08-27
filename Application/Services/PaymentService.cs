using Application.Abstraction;
using Domain.Entities;
public class PaymentService : IPaymentService
{
    private readonly List<Payment> _payments = new List<Payment>();

    public Payment CreatePayment(decimal amount, string currency)
    {
        var payment = new Payment
        {
            PaymentId = Guid.NewGuid(),
            Amount = amount,
            Currency = currency,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };

        _payments.Add(payment);
        return payment;
    }

    public bool ProcessPayment(Guid paymentId)
    {
        var payment = _payments.FirstOrDefault(p => p.PaymentId == paymentId);
        if (payment == null)
        {
            return false;
        }

        // Simulate payment processing logic
        payment.Status = "Succeeded";
        return true;
    }
    public Payment? GetPaymentById(Guid paymentId)
    {
        var payment = _payments.FirstOrDefault(p => p.PaymentId == paymentId);
        return payment;
    }
}
