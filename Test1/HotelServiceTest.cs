using Application.Abstraction;
using Application.Dtos;
using Domain.Abstractions;
using Domain.Entities;
using Moq;

namespace Test1
{
    public class HotelServiceTest
    {
        private readonly IHotelService _service;
        private readonly Mock<IHotelRepository> _repositoryMock;

        public HotelServiceTest()
        {
            _repositoryMock = new Mock<IHotelRepository>();
            _service = new HotelService(_repositoryMock.Object);
        }
        [Fact]
        public async Task CreateHotelAsync_ThrowsException_WhenHotelAlreadyExists()
        {
            var request = new CreateHotelRequest { HotelId = Guid.NewGuid() };
            _repositoryMock.Setup(repo => repo.GetByIdAsync(request.HotelId))
                .ReturnsAsync(new Hotel());
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateHotelAsync(request));
        }
        [Fact]
        public async Task CreatehotelAsync_AddsHotel_WhenHotelDoesNotExist()
        {
            var request = new CreateHotelRequest { HotelId = Guid.NewGuid(), HotelName = "Test Hotel" };
            _repositoryMock.Setup(repo => repo.GetByIdAsync(request.HotelId))
                .ReturnsAsync((Hotel)null);
            await _service.CreateHotelAsync(request);
            _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Hotel>()), Times.Once);
        }
        [Fact]
        public async Task UpdateHotelAsync_ThrowsKeyNotFoundException_WhenHotelDoesNotExist()
        {
            var request = new UpdateHotelRequest { HotelId = Guid.NewGuid() };
            _repositoryMock.Setup(repo => repo.GetByIdAsync(request.HotelId))
                .ReturnsAsync((Hotel)null);
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateHotelAsync(request));
        }
        [Fact]
        public async Task UpdateHotelAsync_UpdatesHotel_WhenHotelExists()
        {
            var existingHotel = new Hotel { HotelId = Guid.NewGuid(), HotelName = "Old Name" };
            var updateRequest = new UpdateHotelRequest { HotelId = existingHotel.HotelId, HotelName = "Updated Name" };
            _repositoryMock.Setup(repo => repo.GetByIdAsync(updateRequest.HotelId)).ReturnsAsync(existingHotel);
            var result = await _service.UpdateHotelAsync(updateRequest);

            Assert.True(result);
            Assert.Equal("Updated Name", existingHotel.HotelName);
            _repositoryMock.Verify(repo => repo.UpdateAsync(existingHotel), Times.Once);
        }
        [Fact]
        public async Task DeleteHotelAsync_ThrowsKeyNotFoundException_WhenHotelDoesNotExist()
        {
            var hotelId = Guid.NewGuid();
            _repositoryMock.Setup(repo => repo.GetByIdAsync(hotelId)).ReturnsAsync((Hotel)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteHotelAsync(hotelId));
        }

        [Fact]
        public async Task DeleteHotelAsync_DeletesHotel_WhenHotelExists()
        {
            var hotelId = Guid.NewGuid();
            var hotel = new Hotel { HotelId = hotelId };
            _repositoryMock.Setup(repo => repo.GetByIdAsync(hotelId)).ReturnsAsync(hotel);

            await _service.DeleteHotelAsync(hotelId);
            _repositoryMock.Verify(repo => repo.DeleteAsync(hotelId), Times.Once);
        }
        [Fact]
        public async Task GetHotelDetailsByIdAsync_ReturnsHotel_WhenHotelExists()
        {
            var hotelId = Guid.NewGuid();
            var hotel = new Hotel { HotelId = hotelId };
            _repositoryMock.Setup(repo => repo.GetByIdAsync(hotelId)).ReturnsAsync(hotel);

            var result = await _service.GetHotelDetailsByIdAsync(hotelId);
            Assert.Equal(hotel, result);
        }

        [Fact]
        public async Task GetHotelDetailsByIdAsync_ThrowsKeyNotFoundException_WhenHotelDoesNotExist()
        {
            var hotelId = Guid.NewGuid();
            _repositoryMock.Setup(repo => repo.GetByIdAsync(hotelId)).ReturnsAsync((Hotel)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetHotelDetailsByIdAsync(hotelId));
        }
        [Fact]
        public async Task GetHotelsByCityAsync_ReturnsHotels_WhenHotelsExistInCity()
        {
            var city = "Test City";
            var hotels = new List<Hotel> { new Hotel { HotelId = Guid.NewGuid(), City = city } };
            _repositoryMock.Setup(repo => repo.GetByCityAsync(city)).ReturnsAsync(hotels);
            var result = await _service.GetHotelsByCityAsync(city);

            Assert.Equal(hotels, result);
        }

        [Fact]
        public async Task GetHotelsByCityAsync_ReturnsEmptyList_WhenNoHotelsExistInCity()
        {
            var city = "Test City";
            _repositoryMock.Setup(repo => repo.GetByCityAsync(city)).ReturnsAsync(new List<Hotel>());
            var result = await _service.GetHotelsByCityAsync(city);
            Assert.Empty(result);
        }
        [Fact]
        public async Task GetRoomsForHotelId_ReturnsRooms_WhenRoomsExistForHotel()
        {
            var hotelId = Guid.NewGuid();
            var rooms = new List<Room> { new Room { RoomId = Guid.NewGuid(), HotelId = hotelId } };
            _repositoryMock.Setup(repo => repo.GetRoomsForHotelId(hotelId)).ReturnsAsync(rooms);
            var result = await _service.GetRoomsForHotelId(hotelId);
            Assert.Equal(rooms, result);
        }

        [Fact]
        public async Task GetRoomsForHotelId_ReturnsNull_WhenNoRoomsExistForHotel()
        {
            var hotelId = Guid.NewGuid();
            _repositoryMock.Setup(repo => repo.GetRoomsForHotelId(hotelId)).ReturnsAsync(new List<Room>());
            var result = await _service.GetRoomsForHotelId(hotelId);
            Assert.Empty(result);
        }


    }
}
