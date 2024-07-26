using Domain.Entities;

namespace Domain.Abstractions
{
    public interface IRoomRepository
    {
        Task<Room> AddAsync(Room room);
        Task<Room> UpdateAsync(Room room);
        Task DeleteAsync(Room room);
        Task<Room> GetByIdAsync(int roomId);
        Task<IEnumerable<Room>> GetAllAsync();
    }
}
