using Application.Abstraction;
using Domain.Abstractions;
using Domain.Entities;

namespace Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IRoomRepository _roomRepository;

        public ReservationService(IReservationRepository reservationRepository, IRoomRepository roomRepository)
        {
            _reservationRepository = reservationRepository;
            _roomRepository = roomRepository;

        }

        public async Task ConfirmReservationAsync(Reservation reservation)
        {
            await _reservationRepository.AddReservationAsync(reservation);

        }


        public async Task DeleteReservationAsync(Guid resId)
        {
            var res = await _reservationRepository.GetReservationByIdAsync(resId);
            if (res == null)
            {
                throw new KeyNotFoundException("Reservation not found");
            }
            await _reservationRepository.DeleteAsync(resId);
        }

        public async Task<IEnumerable<Reservation>> GetAsync()
        {
            return await _reservationRepository.GetAsync();
        }

        public async Task<Reservation?> GetReservationDetailsByIdAsync(Guid resId)
        {
            var res = await _reservationRepository.GetReservationByIdAsync(resId);
            if (res == null)
            {
                throw new InvalidOperationException("Reservation id does not exist");
            }
            return res;
        }

        public async Task<IEnumerable<Reservation>> GetReservationDetailsByUserIdAsync(Guid userId)
        {
            return await _reservationRepository.GetReservationsforUserId(userId);
        }

        public async Task<decimal> GetRoomPricePerNight(Guid roomId)
        {
            var room = await _roomRepository.GetByIdAsync(roomId);
            if (room is null)
            {
                throw new KeyNotFoundException("Room not found");
            }

            return room.PricePerNight;
        }
    }
}
