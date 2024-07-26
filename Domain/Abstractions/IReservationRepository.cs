using Domain.Entities;

namespace Domain.Abstractions
{
    public interface IReservationRepository
    {
        Task<Reservation> GetReservationByIdAsync(int id);
        Task<Reservation> AddReservationAsync(Reservation reservation);
        Task DeleteAsync(Reservation res);
        Task<IEnumerable<Reservation>> GetReservationsforUserId(int userId);
      
    }
}
