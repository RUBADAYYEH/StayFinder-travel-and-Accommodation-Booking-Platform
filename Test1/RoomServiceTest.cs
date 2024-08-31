using Application.Abstraction;
using Application.Dtos;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Entities.Enums;
using Moq;

namespace Test1
{
    public class RoomServiceTest
    {
        private readonly Mock<IRoomRepository> _roomRepositoryMock;
        private readonly IRoomService _roomService;
        public RoomServiceTest()
        {
            _roomRepositoryMock = new Mock<IRoomRepository>();
            _roomService = new RoomService(_roomRepositoryMock.Object);
        }
        [Fact]
        public async Task CreateRoomAsync_ThrowsException_WhenRoomAlreadyExists()
        {
            var request = new CreateRoomRequest { RoomId = Guid.NewGuid() };
            _roomRepositoryMock.Setup(repo => repo.GetByIdAsync(request.RoomId))
                               .ReturnsAsync(new Room());
            await Assert.ThrowsAsync<InvalidOperationException>(() => _roomService.CreateRoomAsync(request));
        }
        [Fact]
        public async Task CreateRoomAsync_AddsRoom_WhenRoomDoesNotExist()
        {
            var request = new CreateRoomRequest { RoomId = Guid.NewGuid(), HotelId = Guid.NewGuid() };
            _roomRepositoryMock.Setup(repo => repo.GetByIdAsync(request.RoomId))
                               .ReturnsAsync((Room)null);

            await _roomService.CreateRoomAsync(request);
            _roomRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Room>()), Times.Once);
        }
        [Fact]
        public async Task DeleteRoomAsync_ThrowsException_WhenRoomDoesNotExist()
        {
            var roomId = Guid.NewGuid();
            _roomRepositoryMock.Setup(repo => repo.GetByIdAsync(roomId))
                               .ReturnsAsync((Room)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _roomService.DeleteRoomAsync(roomId));
        }
        [Fact]
        public async Task DeleteRoomAsync_DeletesRoom_WhenRoomExists()
        {
            var roomId = Guid.NewGuid();
            _roomRepositoryMock.Setup(repo => repo.GetByIdAsync(roomId))
                               .ReturnsAsync(new Room { RoomId = roomId });

            await _roomService.DeleteRoomAsync(roomId);
            _roomRepositoryMock.Verify(repo => repo.DeleteAsync(roomId), Times.Once);
        }

        [Fact]
        public void GetAllAsync_ReturnsAllRooms()
        {
            var rooms = new List<Room> { new Room { RoomId = Guid.NewGuid() } }.AsQueryable();
            _roomRepositoryMock.Setup(repo => repo.GetAllAsync())
                               .Returns(rooms);
            var result = _roomService.GetAllAsync();
            Assert.Equal(rooms, result);
        }
        [Fact]
        public async Task GetById_ReturnsRoom_WhenRoomExists()
        {
            var roomId = Guid.NewGuid();
            var room = new Room { RoomId = roomId };
            _roomRepositoryMock.Setup(repo => repo.GetByIdAsync(roomId))
                               .ReturnsAsync(room);
            var result = await _roomService.GetById(roomId);
            Assert.Equal(room, result);
        }
        [Fact]
        public async Task GetById_ReturnsNull_WhenRoomDoesNotExist()
        {
            var roomId = Guid.NewGuid();
            _roomRepositoryMock.Setup(repo => repo.GetByIdAsync(roomId))
                               .ReturnsAsync((Room)null);
            var result = await _roomService.GetById(roomId);
            Assert.Null(result);
        }
        [Fact]
        public async Task GetRoomDetailsByIdAsync_ThrowsException_WhenRoomDoesNotExist()
        {
            var roomId = Guid.NewGuid();
            _roomRepositoryMock.Setup(repo => repo.GetByIdAsync(roomId))
                               .ReturnsAsync((Room)null);
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _roomService.GetRoomDetailsByIdAsync(roomId));
        }

        [Fact]
        public async Task GetRoomDetailsByIdAsync_ReturnsRoom_WhenRoomExists()
        {
            var roomId = Guid.NewGuid();
            var room = new Room { RoomId = roomId };
            _roomRepositoryMock.Setup(repo => repo.GetByIdAsync(roomId))
                               .ReturnsAsync(room);
            var result = await _roomService.GetRoomDetailsByIdAsync(roomId);
            Assert.Equal(room, result);
        }
        [Fact]
        public void SearchRoomsAsync_ReturnsAllRooms_WhenNoFiltersAreApplied()
        {
            var rooms = new List<Room>
            {
               new Room { RoomId = Guid.NewGuid(), RoomType = RoomType.Standard, NumberOfAdults = 2, NumberOfChildren = 1, PricePerNight = 100 },
               new Room { RoomId = Guid.NewGuid(), RoomType = RoomType.Luxury, NumberOfAdults = 3, NumberOfChildren = 2, PricePerNight = 200 }
            }.AsQueryable();

            _roomRepositoryMock.Setup(repo => repo.GetAllAsync()).Returns(rooms);
            var request = new SearchRoomRequest();
            var result = _roomService.SearchRoomsAsync(request);
            Assert.Equal(rooms.Count(), result.Count());
        }


        [Fact]
        public void SearchRoomsAsync_FiltersByRoomType()
        {
            var rooms = GetTestRooms();
            _roomRepositoryMock.Setup(repo => repo.GetAllAsync()).Returns(rooms.AsQueryable());
            var request = new SearchRoomRequest
            {
                RoomType = RoomType.Luxury
            };
            var result = _roomService.SearchRoomsAsync(request);
            Assert.All(result, r => Assert.Equal(RoomType.Luxury, r.RoomType));
        }

        [Fact]
        public void SearchRoomsAsync_FiltersByDateRange()
        {
            var rooms = GetTestRooms();
            _roomRepositoryMock.Setup(repo => repo.GetAllAsync()).Returns(rooms.AsQueryable());
            var request = new SearchRoomRequest
            {
                CheckInDate = new DateTime(2024, 9, 1),
                CheckOutDate = new DateTime(2024, 9, 10)
            };
            var result = _roomService.SearchRoomsAsync(request);

            Assert.All(result, r => Assert.True(r.Reservations == null || !r.Reservations.Any(reservation =>
                reservation.CheckInDate < request.CheckOutDate.Value && reservation.CheckOutDate > request.CheckInDate.Value)));
        }

        [Fact]
        public void SearchRoomsAsync_FiltersByNumberOfAdults()
        {
            var rooms = GetTestRooms();
            _roomRepositoryMock.Setup(repo => repo.GetAllAsync()).Returns(rooms.AsQueryable());

            var request = new SearchRoomRequest
            {
                NumberOfAdults = 2
            };
            var result = _roomService.SearchRoomsAsync(request);

            Assert.All(result, r => Assert.True(r.NumberOfAdults >= request.NumberOfAdults));
        }

        [Fact]
        public void SearchRoomsAsync_FiltersByPriceRange()
        {
            var rooms = GetTestRooms();
            _roomRepositoryMock.Setup(repo => repo.GetAllAsync()).Returns(rooms.AsQueryable());
            var request = new SearchRoomRequest
            {
                MinPricePerNight = 100,
                MaxPricePerNight = 200
            };
            var result = _roomService.SearchRoomsAsync(request);

            Assert.All(result, r => Assert.InRange(r.PricePerNight, request.MinPricePerNight.Value, request.MaxPricePerNight.Value));
        }

        [Fact]
        public void SearchRoomsAsync_ReturnsEmptyList_WhenNotEnoughRooms()
        {
            var rooms = GetTestRooms();
            _roomRepositoryMock.Setup(repo => repo.GetAllAsync()).Returns(rooms.AsQueryable());

            var request = new SearchRoomRequest
            {
                NumberOfRooms = 10
            };

            var result = _roomService.SearchRoomsAsync(request);
            Assert.Empty(result);
        }

        private List<Room> GetTestRooms()
        {
            return new List<Room>
        {
            new Room
            {
                RoomId = Guid.NewGuid(),
                RoomType = RoomType.Luxury,
                NumberOfAdults = 2,
                NumberOfChildren = 1,
                PricePerNight = 150,
                Reservations = new List<Reservation>
                {
                    new Reservation
                    {
                        CheckInDate = new DateTime(2024, 9, 5),
                        CheckOutDate = new DateTime(2024, 9, 8)
                    }
                }
            },
            new Room
            {
                RoomId = Guid.NewGuid(),
                RoomType = RoomType.Standard,
                NumberOfAdults = 1,
                NumberOfChildren = 0,
                PricePerNight = 80,
                Reservations = new List<Reservation>
                {
                    new Reservation
                    {
                        CheckInDate = new DateTime(2024, 9, 12),
                        CheckOutDate = new DateTime(2024, 9, 15)
                    }
                }
            },

        };
        }
        [Fact]
        public async Task UpdateRoomAsync_ThrowsException_WhenRoomDoesNotExist()
        {
            var request = new UpdateRoomRequest { RoomId = Guid.NewGuid() };
            _roomRepositoryMock.Setup(repo => repo.GetByIdAsync(request.RoomId))
                               .ReturnsAsync((Room)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _roomService.UpdateRoomAsync(request));
        }

        [Fact]
        public async Task UpdateRoomAsync_UpdatesRoom_WhenRoomExists()
        {
            var room = new Room { RoomId = Guid.NewGuid() };
            var request = new UpdateRoomRequest { RoomId = room.RoomId, NumberOfAdults = 3 };

            _roomRepositoryMock.Setup(repo => repo.GetByIdAsync(request.RoomId))
                               .ReturnsAsync(room);
            await _roomService.UpdateRoomAsync(request);

            Assert.Equal(request.NumberOfAdults, room.NumberOfAdults);
            _roomRepositoryMock.Verify(repo => repo.UpdateAsync(room), Times.Once);
        }

    }
}