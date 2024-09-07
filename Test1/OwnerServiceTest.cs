using Application.Abstraction;
using Application.Dtos;
using Application.Services;
using Domain.Abstractions;
using Domain.Entities;
using Moq;

namespace Test1
{
    public class OwnerServiceTest
    {
        private readonly Mock<IOwnerRepository> _mockRepository;
        private readonly IOwnerService _ownerService;
        
        public OwnerServiceTest()
        {
            _mockRepository = new Mock<IOwnerRepository>();
            _ownerService = new OwnerService(_mockRepository.Object);
        }
        [Fact]
        public async Task CreateOwnerAsync_ThrowsException_WhenOwnerAlreadyExists()
        {
            var requet = new CreateOwnerRequest { OwnerId = new Guid()};
            _mockRepository.Setup(repo => repo.GetByIdAsync(requet.OwnerId))
                .ReturnsAsync(new Owner());

            await Assert.ThrowsAsync<InvalidOperationException>(() => _ownerService.CreateOwnerAsync(requet));
        }
        [Fact]
        public async Task CreateOwnerAsync_Creates_WhenOwnerDoesNotExist()
        {
            var requet = new CreateOwnerRequest { OwnerId = new Guid(), OwnerName="Name" };
            _mockRepository.Setup(repo => repo.GetByIdAsync(requet.OwnerId))
                .ReturnsAsync((Owner)null);

            await _ownerService.CreateOwnerAsync(requet);
            _mockRepository.Verify(repo => repo.AddAsync(It.IsAny<Owner>()), Times.Once);
        }
        [Fact]
        public async Task DeleteOwnerAsync_ThrowsException_WhenOwnerDoesNotExist()
        {
            var ownerId = new Guid();
            _mockRepository.Setup(repo => repo.GetByIdAsync(ownerId))
                .ReturnsAsync((Owner)null);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _ownerService.DeleteOwnerAsync(ownerId));
        }
        [Fact]
        public async Task DeleteOwnerAsync_Deletes_WhenOwnerExists()
        {
            var ownerId = new Guid();
          
            var owner = new Owner { OwnerId = ownerId };
            _mockRepository.Setup(repo => repo.GetByIdAsync(ownerId))
              .ReturnsAsync(owner);
            await _ownerService.DeleteOwnerAsync(ownerId);
            _mockRepository.Verify(repo => repo.DeleteAsync(ownerId), Times.Once);
        }
        [Fact]
        public async Task GetAllAsync_ReturnsAllOwners()
        {
            var owners = new List<Owner> { new Owner { OwnerId = Guid.NewGuid(), OwnerName = "Owner1" } };
            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(owners);
            var result = await _ownerService.GetAllAsync();
            Assert.Equal(owners, result);
        }
        [Fact]
        public async Task GetById_ReturnsOwner_WhenOwnerExists()
        {
            var ownerId = Guid.NewGuid();
            var owner = new Owner { OwnerId = ownerId };
            _mockRepository.Setup(repo => repo.GetByIdAsync(ownerId)).ReturnsAsync(owner);
            var result = await _ownerService.GetById(ownerId);
            Assert.Equal(owner, result);
        }

        [Fact]
        public async Task GetById_ReturnsNull_WhenOwnerDoesNotExist()
        {
            var ownerId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.GetByIdAsync(ownerId)).ReturnsAsync((Owner)null);

            var result = await _ownerService.GetById(ownerId);
            Assert.Null(result);
        }
        [Fact]
        public async Task UpdateOwnerAsync_ThrowsKeyNotFoundException_WhenOwnerDoesNotExist()
        {
            var updateRequest = new UpdateOwnerRequest { OwnerId = Guid.NewGuid(), OwnerName = "Updated Name" };
            _mockRepository.Setup(repo => repo.GetByIdAsync(updateRequest.OwnerId)).ReturnsAsync((Owner)null);
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _ownerService.UpdateOwnerAsync(updateRequest));
        }

        [Fact]
        public async Task UpdateOwnerAsync_UpdatesOwner_WhenOwnerExists()
        {
            var existingOwner = new Owner { OwnerId = Guid.NewGuid(), OwnerName = "Old Name" };
            var updateRequest = new UpdateOwnerRequest { OwnerId = existingOwner.OwnerId, OwnerName = "Updated Name" };
            _mockRepository.Setup(repo => repo.GetByIdAsync(updateRequest.OwnerId)).ReturnsAsync(existingOwner);

            var result = await _ownerService.UpdateOwnerAsync(updateRequest);

            Assert.True(result);
            Assert.Equal("Updated Name", existingOwner.OwnerName);
            _mockRepository.Verify(repo => repo.UpdateAsync(existingOwner), Times.Once);
        }

    }
}
