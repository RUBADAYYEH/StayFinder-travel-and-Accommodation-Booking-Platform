using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TrendingRepository : ITrendingRepository
    {
        private readonly StayFinderDbContext _context;

        public TrendingRepository(StayFinderDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<string>> GetTrendingCities()
        {
            var res = await _context.Reservations
              .Include(e => e.room)
              .ThenInclude(r => r.Hotel)
              .GroupBy(r => r.room.Hotel.City)
              .Select(g => new
              {
                  City = g.Key,
                  HotelCount = g.Select(r => r.room.Hotel.HotelId).Distinct().Count()
              })
              .OrderByDescending(c => c.HotelCount)
              .Take(5)
              .Select(c => c.City)
              .ToListAsync();

            return res;
        }
    }
}
