using Domain.Entities;
namespace Domain.Abstractions
{
    public interface IOwnerRepository
    {
        Task AddAsync(Owner owner);
        Task UpdateAsync(Owner owner);
        Task DeleteAsync(int ownerId);
        Task<Owner?> GetByIdAsync(int ownerId);
        Task<IEnumerable<Owner>> GetAllAsync();
    }
}
