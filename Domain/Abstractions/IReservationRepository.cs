using Domain.Entities;

namespace Domain.Abstractions
{
    public interface IReservationRepository
    {
        Task<IEnumerable<Reservation>> GetAsync();
        Task<Reservation?> GetReservationByIdAsync(int id);
        Task AddReservationAsync(Reservation reservation);
        Task DeleteAsync(int resId);
        Task<IEnumerable<Reservation>> GetReservationsforUserId(int userId);
    }
}
