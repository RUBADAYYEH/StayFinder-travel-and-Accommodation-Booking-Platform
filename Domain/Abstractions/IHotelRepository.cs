using Domain.Entities;
namespace Domain.Abstractions
{
    public interface IHotelRepository
    {
        Task AddAsync(Hotel hotel);
        Task UpdateAsync(Hotel hotel);
        Task DeleteAsync(Guid hotelId);
        Task<Hotel?> GetByIdAsync(Guid hotelId);
        Task<IEnumerable<Hotel>> GetByCityAsync(string city);
        Task<IEnumerable<Hotel>> GetAllAsync();
        Task<IEnumerable<Room>> GetRoomsForHotelId(Guid hotelId);
        Task<IEnumerable<string>> GetCities();
    }
}
