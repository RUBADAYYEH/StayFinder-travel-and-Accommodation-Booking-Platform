﻿using Application.Abstraction;
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
            var ow= await _ownerRepository.GetByIdAsync(request.OwnerId);
            if (ow != null)
            {
                throw new InvalidOperationException("Owner with id already exist");
            }
            var owner = new Owner() { OwnerId = request.OwnerId, OwnerName = request.OwnerName };
            await _ownerRepository.AddAsync(owner);
        }

        public async Task DeleteOwnerAsync(Guid ownerId)
        {
            var owner = await _ownerRepository.GetByIdAsync(ownerId);
            if (owner == null)
            {
                throw new InvalidOperationException("Owner with id already exist");
            }
      
            await _ownerRepository.DeleteAsync(ownerId);
        }

        public async Task<IEnumerable<Owner>> GetAllAsync()
        {
            return await _ownerRepository.GetAllAsync();
        }

        public async Task<Owner?> GetById(Guid ownerId)
        {
            var owner = await _ownerRepository.GetByIdAsync(ownerId);
            if (owner != null)
            {
                return owner;
            }
            return null;
        }

        async Task<bool> IOwnerService.UpdateOwnerAsync(UpdateOwnerRequest request)
        {
            var owner = await _ownerRepository.GetByIdAsync(request.OwnerId);
            if (owner == null)
            {
                throw new KeyNotFoundException("Owner not found");
            }
            if (!string.IsNullOrEmpty(request.OwnerName))
            {
                owner.OwnerName = request.OwnerName;

                await _ownerRepository.UpdateAsync(owner);

            }
            return true; //update was successful
        }
    }
}
