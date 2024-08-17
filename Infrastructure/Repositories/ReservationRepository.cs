using Domain.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly StayFinderDbContext _context;

        public ReservationRepository(StayFinderDbContext context)
        {
            _context = context;
        }
        public async Task AddReservationAsync(Reservation reservation)
        {
            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int resId)
        {
            var res = await _context.Reservations.FindAsync(resId);
            if (res != null)
            {
                _context.Reservations.Remove(res);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Reservation>> GetAsync()
        {
            return await _context.Reservations.ToListAsync();
        }
        public async Task<Reservation?> GetReservationByIdAsync(int id)
        {
            return await _context.Reservations.FindAsync(id);
        }
        public Task<IEnumerable<Reservation>> GetReservationsforUserId(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
