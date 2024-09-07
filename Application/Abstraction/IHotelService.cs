using Application.Dtos;
using Domain.Entities;

namespace Application.Abstraction
{
    public interface IHotelService
    {
        Task CreateHotelAsync(CreateHotelRequest request);
        Task<bool> UpdateHotelAsync(UpdateHotelRequest updateRequest);
        Task DeleteHotelAsync(Guid hotelId);
        Task<Hotel?> GetHotelDetailsByIdAsync(Guid hotelId);
        Task<IEnumerable<Hotel>> GetHotelsByCityAsync(string city);
        Task<IEnumerable<Hotel>> GetAllAsync();
        Task<Hotel?> GetById(Guid hotelId);
        Task<IEnumerable<Room>?> GetRoomsForHotelId(Guid hotelId);
        Task<IEnumerable<string>> GetCities();
    }
}
