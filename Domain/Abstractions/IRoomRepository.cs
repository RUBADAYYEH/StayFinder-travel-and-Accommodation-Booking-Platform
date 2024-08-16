using Domain.Entities;

namespace Domain.Abstractions
{
    public interface IRoomRepository
    {
        Task AddAsync(Room room);
        Task UpdateAsync(Room room);
        Task DeleteAsync(int roomId);
        Task<Room> GetByIdAsync(int roomId);
        IQueryable<Room> GetAllAsync();
       
    }
}
