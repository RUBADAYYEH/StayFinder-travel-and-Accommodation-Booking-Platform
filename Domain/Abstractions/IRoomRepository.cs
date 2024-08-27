using Domain.Entities;
namespace Domain.Abstractions
{
    public interface IRoomRepository
    {
        Task AddAsync(Room room);
        Task UpdateAsync(Room room);
        Task DeleteAsync(Guid roomId);
        Task<Room?> GetByIdAsync(Guid roomId);
        IQueryable<Room> GetAllAsync();
    }
}
