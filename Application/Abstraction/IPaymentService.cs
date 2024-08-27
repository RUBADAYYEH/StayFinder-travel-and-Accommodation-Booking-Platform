using Domain.Entities;
namespace Application.Abstraction
{
    public interface IPaymentService
    {
        Payment CreatePayment(decimal amount, string currency);
        bool ProcessPayment(Guid paymentId);
        Payment? GetPaymentById(Guid paymentId);
    }
}
