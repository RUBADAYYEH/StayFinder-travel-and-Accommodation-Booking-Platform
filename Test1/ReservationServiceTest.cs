using Application.Abstraction;
using Application.Services;
using Domain.Abstractions;
using Domain.Entities;
using Moq;

namespace Test1
{
    public class ReservationServiceTest
    {
        private readonly Mock<IReservationRepository> _reservationRepositoryMock;
        private readonly IReservationService _reservationService;
        private readonly Mock<IRoomRepository> _roomRepositoryMock;

        public ReservationServiceTest()
        {
            _reservationRepositoryMock = new Mock<IReservationRepository>();
            _roomRepositoryMock = new Mock<IRoomRepository>();
            _reservationService = new ReservationService(_reservationRepositoryMock.Object, _roomRepositoryMock.Object);
        }
        [Fact]
        public async Task DeleteReservationAsync_ThrowsException_WhenIdNotFound()
        {
            var id = Guid.NewGuid();
            _reservationRepositoryMock.Setup(repo => repo.GetReservationByIdAsync(id)).ReturnsAsync((Reservation)null);
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _reservationService.DeleteReservationAsync(id));
        }
        [Fact]
        public async Task DeleteReservationAsync_Delets_WhenIdFound()
        {
            var id = Guid.NewGuid();
            _reservationRepositoryMock.Setup(repo => repo.GetReservationByIdAsync(id)).ReturnsAsync(new Reservation());

            await _reservationService.DeleteReservationAsync(id);
            _reservationRepositoryMock.Verify(repo => repo.DeleteAsync(id), Times.Once);
        }

    }
}
