using Application.Dtos;
using Domain.Entities;

namespace Application.Abstraction
{
    public interface IReservationService
    {
        Task<IEnumerable<Reservation>> GetAsync();
        Task CreateReservationAsync(CreateReservationRequest request);
        Task DeleteReservationAsync(Guid resId);
        Task<Reservation?> GetReservationDetailsByIdAsync(Guid resId);
        Task<IEnumerable<Reservation>> GetReservationDetailsByUserIdAsync(Guid userId);
        Task ConfirmReservationAsync(Guid reservationId);
    }
}
