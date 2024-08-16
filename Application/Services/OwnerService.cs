using Application.Abstraction;
using Application.Dtos;
using Domain.Abstractions;
using Domain.Entities;

namespace Application.Services
{
    public class OwnerService : IOwnerService
    {
        private readonly IOwnerRepository _ownerRepository;

        public OwnerService(IOwnerRepository ownerRepository)
        {
            _ownerRepository = ownerRepository;
        }

        public async Task CreateOwnerAsync(CreateOwnerRequest request)
        {
            var owner = new Owner() { OwnerId=request.OwnerId, OwnerName=request.OwnerName};
            await _ownerRepository.AddAsync(owner);
        }

        public async Task DeleteOwnerAsync(int ownerId)
        {
            var owner = _ownerRepository.GetByIdAsync(ownerId);
            if (owner != null)
            {
                await _ownerRepository.DeleteAsync(ownerId);  
            }
        }

        public async Task<IEnumerable<Owner>> GetAllAsync()
        {
            return await _ownerRepository.GetAllAsync();
        }

        public async Task<Owner> GetById(int ownerId)
        {
            var owner = await _ownerRepository.GetByIdAsync(ownerId);
            if (owner != null)
            {
                return owner;
            }
            return null;
        }

        public Task<Owner> GetOwnerDetailsByIdAsync(int ownerId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateOwnerAsync(UpdateOwnerRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
