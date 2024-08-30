using Domain.Entities;

namespace Application.Abstraction
{
    public interface IReservationService
    {
        Task<IEnumerable<Reservation>> GetAsync();
        Task<Decimal> GetRoomPricePerNight(Guid roomId);
        Task DeleteReservationAsync(Guid resId);
        Task<Reservation?> GetReservationDetailsByIdAsync(Guid resId);
        Task<IEnumerable<Reservation>> GetReservationDetailsByUserIdAsync(Guid userId);
        Task ConfirmReservationAsync(Reservation reservation);

    }
}
