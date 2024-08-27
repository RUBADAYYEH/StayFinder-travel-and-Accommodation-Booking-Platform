using Domain.Entities;
namespace Domain.Abstractions
{
    public interface IOwnerRepository
    {
        Task AddAsync(Owner owner);
        Task UpdateAsync(Owner owner);
        Task DeleteAsync(Guid ownerId);
        Task<Owner?> GetByIdAsync(Guid ownerId);
        Task<IEnumerable<Owner>> GetAllAsync();
    }
}
