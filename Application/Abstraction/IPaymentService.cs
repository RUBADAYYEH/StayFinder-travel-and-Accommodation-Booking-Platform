using Domain.Entities;
namespace Application.Abstraction
{
    public interface IPaymentService
    {
        Task<Payment> CreatePayment(decimal amount, string currency);
        Task<bool> ProcessPayment(Guid paymentId);
        Task<Payment> GetPaymentById(Guid paymentId);
    }
}
