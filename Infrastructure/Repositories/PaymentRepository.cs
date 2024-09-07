using Domain.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly StayFinderDbContext _context;

        public PaymentRepository(StayFinderDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Guid paymentId)
        {
            var payment = await _context.Payments.FindAsync(paymentId);
            if (payment != null)
            {
                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            return await _context.Payments.ToListAsync();
        }
        public async Task<Payment> GetByIdAsync(Guid paymentId)
        {
            return await _context.Payments.FirstAsync(p => p.PaymentId == paymentId);
        }
        public async Task UpdateAsync(Payment payment)
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
        }
    }
}
