using Application.Dtos;
using Domain.Entities;

namespace Application.Abstraction
{
    public interface IReservationService
    {
        Task<IEnumerable<Reservation>> GetAsync();
        Task CreateReservationAsync(CreateReservationRequest request);
        Task DeleteReservationAsync(int resId);
        Task<Reservation?> GetReservationDetailsByIdAsync(int resId);
        Task<IEnumerable<Reservation>> GetReservationDetailsByUserIdAsync(int userId);
    }
}
