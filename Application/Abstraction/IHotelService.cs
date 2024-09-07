using Application.Dtos;
using Domain.Entities;

namespace Application.Abstraction
{
    public interface IHotelService
    {
        Task CreateHotelAsync(CreateHotelRequest request);
        Task<bool> UpdateHotelAsync(UpdateHotelRequest updateRequest);
        Task DeleteHotelAsync(int hotelId);
        Task<Hotel?> GetHotelDetailsByIdAsync(int hotelId);
        Task<IEnumerable<Hotel>> GetHotelsByCityAsync(string city);
        Task<IEnumerable<Hotel>> GetAllAsync();
        Task<Hotel?> GetById(int hotelId);
        Task<IEnumerable<Room>?> GetRoomsForHotelId(int hotelId);
        Task<IEnumerable<string>> GetCities();
    }
}
