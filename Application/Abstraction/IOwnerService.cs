using Application.Dtos;
using Domain.Entities;

namespace Application.Abstraction
{
    public interface IOwnerService
    {
        Task CreateOwnerAsync(CreateOwnerRequest request);
        Task<bool> UpdateOwnerAsync(UpdateOwnerRequest request);
        Task DeleteOwnerAsync(Guid ownerId);
        Task<IEnumerable<Owner>> GetAllAsync();
        Task<Owner?> GetById(Guid ownerId);
    }
}
