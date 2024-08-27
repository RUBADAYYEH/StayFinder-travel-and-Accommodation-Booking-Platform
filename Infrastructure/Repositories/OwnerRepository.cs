using Domain.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly StayFinderDbContext _context;

        public OwnerRepository(StayFinderDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Owner owner)
        {
            await _context.Owners.AddAsync(owner);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Guid ownerId)
        {
            var owner = await _context.Owners.FindAsync(ownerId);
            if (owner != null)
            {
                _context.Owners.Remove(owner);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Owner>> GetAllAsync()
        {
            return await _context.Owners.ToListAsync();
        }
        public async Task<Owner?> GetByIdAsync(Guid ownerId)
        {
            return await _context.Owners.FindAsync(ownerId);
        }
        public async Task UpdateAsync(Owner owner)
        {
            _context.Owners.Update(owner);
            await _context.SaveChangesAsync();
        }
    }
}
