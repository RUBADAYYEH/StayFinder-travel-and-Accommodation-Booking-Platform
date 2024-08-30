using Domain.Entities;
namespace Domain.Abstractions
{
    public interface IPaymentRepository
    {
        Task AddAsync(Payment payment);
        Task DeleteAsync(Guid paymentId);
        Task<IEnumerable<Payment>> GetAllAsync();
        Task<Payment> GetByIdAsync(Guid paymentId);
        Task UpdateAsync(Payment payment);
    }
}
