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
        private readonly IPaymentService _paymentService;

        private List<Reservation> tempReservations = new List<Reservation>();
        public ReservationService(IReservationRepository reservationRepository, IRoomRepository roomRepository, IPaymentService paymentService)
        {
            _reservationRepository = reservationRepository;
            _roomRepository = roomRepository;
            _paymentService = paymentService;
        }
        public async Task CreateReservationAsync(CreateReservationRequest request)
        {
            if (await _reservationRepository.GetReservationByIdAsync(request.ReservationId) != null)
            {
                throw new HttpRequestException("Existing Reservation with id found");
            }

            var room = await _roomRepository.GetByIdAsync(request.RoomId);
            if (room is null)
            {
                throw new KeyNotFoundException("Room not found");
            }
            var TotalFees = ((request.CheckOutDate - request.CheckInDate).Days) * room.PricePerNight;
            var res = new Reservation { ReservationId = request.ReservationId, PaymentId=request.PaymentId, RoomId = request.RoomId, CheckInDate = request.CheckInDate, CheckOutDate = request.CheckOutDate, TotalFees = TotalFees };
         
            tempReservations.Add(res);
        }
        public async Task ConfirmReservationAsync(Guid reservationId)
        {
            var reservation = tempReservations.FirstOrDefault(r => r.ReservationId==reservationId);
            if (reservation == null)
            {
                throw new KeyNotFoundException("Reservation not found");
            }

            var payment = _paymentService.GetPaymentById(reservation.PaymentId);
            if (payment == null || payment.Status != "Succeeded")
            {
                throw new InvalidOperationException("Payment not completed or failed");
            }

            await _reservationRepository.AddReservationAsync(reservation);

        }
       

        public async Task DeleteReservationAsync(Guid resId)
        {
            var res = _reservationRepository.GetReservationByIdAsync(resId);
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
            return await _reservationRepository.GetReservationByIdAsync(resId);
        }

        public async Task<IEnumerable<Reservation>> GetReservationDetailsByUserIdAsync(Guid userId)
        {
            return await _reservationRepository.GetReservationsforUserId(userId);
        }

    }
}
