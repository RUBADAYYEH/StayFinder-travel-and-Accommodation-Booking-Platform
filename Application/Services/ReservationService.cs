using Application.Abstraction;
using Application.Dtos;
using Domain.Abstractions;
using Domain.Entities;

namespace Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IRoomRepository _roomRepository;
        public ReservationService(IReservationRepository reservationRepository , IRoomRepository roomRepository)
        {
            _reservationRepository = reservationRepository;
            _roomRepository = roomRepository;
        }
        public async Task CreateReservationAsync(CreateReservationRequest request)
        {
            if (await _reservationRepository.GetReservationByIdAsync(request.ReservationId) != null)
            {
                throw new HttpRequestException("Existing Reservation with id found");
            }
          
            var room= await _roomRepository.GetByIdAsync(request.RoomId);
            var TotalFees = ((request.CheckOutDate - request.CheckInDate).Days)*room.PricePerNight;
            var res = new Reservation { ReservationId = request.ReservationId, RoomId = request.RoomId, CheckInDate = request.CheckInDate, CheckOutDate = request.CheckOutDate, TotalFees = TotalFees };
             await _reservationRepository.AddReservationAsync(res);
        }

        public async Task DeleteReservationAsync(int resId)
        {
             var res = _reservationRepository.GetReservationByIdAsync(resId);
            if (res == null)
            {
                throw new KeyNotFoundException("Reservation not found");
            }
             await _reservationRepository.DeleteAsync(res.Result);
        }

        public async Task<Reservation> GetReservationDetailsByIdAsync(int resId)
        {
            return await  _reservationRepository.GetReservationByIdAsync(resId);
        }

        public async Task<IEnumerable<Reservation>> GetReservationDetailsByUserIdAsync(int userId)
        {
            return await _reservationRepository.GetReservationsforUserId(userId);
        }

      
    }
}
