using Domain.Entities;
namespace Domain.Abstractions
{
    public interface IHotelRepository
    {
        Task AddAsync(Hotel hotel);
        Task UpdateAsync(Hotel hotel);
        Task DeleteAsync(int hotelId);
        Task<Hotel?> GetByIdAsync(int hotelId);
        Task<IEnumerable<Hotel>> GetByCityAsync(string city);
        Task<IEnumerable<Hotel>> GetAllAsync();
        Task<IEnumerable<Room>> GetRoomsForHotelId(int hotelId);
        Task<IEnumerable<string>> GetCities();
    }
}
