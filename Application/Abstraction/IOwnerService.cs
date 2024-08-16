using Application.Dtos;
using Domain.Entities;

namespace Application.Abstraction
{
    public interface IOwnerService
    {
        Task CreateOwnerAsync(CreateOwnerRequest request);
        Task UpdateOwnerAsync(UpdateOwnerRequest request);
        Task DeleteOwnerAsync(int ownerId);
        Task<Owner> GetOwnerDetailsByIdAsync(int ownerId);
        
        Task<IEnumerable<Owner>> GetAllAsync();
        Task<Owner> GetById(int ownerId);
    }
}
