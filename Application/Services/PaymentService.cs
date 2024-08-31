using Application.Abstraction;
using Domain.Abstractions;
using Domain.Entities;
public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;

    public PaymentService(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<Payment> CreatePayment(decimal amount, string currency)
    {
        var payment = new Payment
        {
            Amount = amount,
            Currency = currency,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };

        await _paymentRepository.AddAsync(payment);
        return payment;
    }

    public async Task<bool> ProcessPayment(Guid paymentId)
    {
        var payment = await _paymentRepository.GetByIdAsync(paymentId);
        if (payment == null)
        {
            return false;
        }
        payment.Status = "Succeeded";
        await _paymentRepository.UpdateAsync(payment);
        return true;
    }
    public async Task<Payment> GetPaymentById(Guid paymentId)
    {
        var payment = await _paymentRepository.GetByIdAsync(paymentId);
        if (payment == null)
        {
            throw new InvalidOperationException("Payment Id not found");
        }
        return payment;
    }


}
