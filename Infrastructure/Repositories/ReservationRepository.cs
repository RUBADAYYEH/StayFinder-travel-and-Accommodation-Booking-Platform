using Domain.Abstractions;
using Domain.Entities;

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

        public Task DeleteAsync(Reservation res)
        {
            throw new NotImplementedException();
        }

        public async Task<Reservation> GetReservationByIdAsync(int id)
        {
            return await _context.Reservations.FindAsync(id);
        }

        public Task<IEnumerable<Reservation>> GetReservationsforUserId(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
