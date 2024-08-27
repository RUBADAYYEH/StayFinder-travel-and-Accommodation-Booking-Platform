using Domain.Entities;

namespace Domain.Abstractions
{
    public interface IReservationRepository
    {
        Task<IEnumerable<Reservation>> GetAsync();
        Task<Reservation?> GetReservationByIdAsync(Guid id);
        Task AddReservationAsync(Reservation reservation);
        Task DeleteAsync(Guid resId);
        Task<IEnumerable<Reservation>> GetReservationsforUserId(Guid userId);
    }
}
