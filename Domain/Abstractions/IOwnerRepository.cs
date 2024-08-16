using Domain.Entities;

namespace Domain.Abstractions
{
    public interface IOwnerRepository
    {
        Task AddAsync(Owner hotel);
        Task UpdateAsync(Hotel hotel);
        Task DeleteAsync(int ownerId);
        Task<Owner> GetByIdAsync(int ownerId);
        Task<IEnumerable<Owner>> GetAllAsync();
    }
}
