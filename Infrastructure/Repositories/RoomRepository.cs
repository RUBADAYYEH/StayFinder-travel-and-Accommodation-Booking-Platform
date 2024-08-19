using Domain.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly StayFinderDbContext _context;

        public RoomRepository(StayFinderDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Room room)
        {
            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int roomId)
        {

            var room = await _context.Rooms.FindAsync(roomId);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
            }
        }
        public IQueryable<Room> GetAllAsync()
        {
            return _context.Rooms
                   .Include(r => r.Reservations).Include(b => b.Hotel)
                   .AsQueryable();
        }
        public async Task<Room?> GetByIdAsync(int roomId)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room != null)
            {
                return room;
            }
            return null;
        }
        public async Task UpdateAsync(Room room)
        {
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
        }
    }
}
