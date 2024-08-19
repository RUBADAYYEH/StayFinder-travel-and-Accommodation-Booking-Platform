using Domain.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class HotelRepository : IHotelRepository
    {
        private readonly StayFinderDbContext _context;

        public HotelRepository(StayFinderDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Hotel hotel)
        {
            await _context.Hotels.AddAsync(hotel);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int hotelId)
        {
            var hotel = await _context.Hotels.FindAsync(hotelId);
            if (hotel != null)
            {
                _context.Hotels.Remove(hotel);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Hotel>> GetAllAsync()
        {
            return await _context.Hotels.ToListAsync();
        }
        public async Task<IEnumerable<Hotel>> GetByCityAsync(string city)
        {
            return await _context.Hotels.Where(h => h.City == city).ToListAsync();
        }
        public async Task<Hotel?> GetByIdAsync(int hotelId)
        {
            return await _context.Hotels.FindAsync(hotelId);
        }
        public async Task<IEnumerable<Room>> GetRoomsForHotelId(int hotelId)
        {
            return await _context.Rooms.Where(r => r.HotelId == hotelId).ToListAsync();
        }
        public async Task UpdateAsync(Hotel hotel)
        {
            _context.Hotels.Update(hotel);
            await _context.SaveChangesAsync();
        }


    }
}
