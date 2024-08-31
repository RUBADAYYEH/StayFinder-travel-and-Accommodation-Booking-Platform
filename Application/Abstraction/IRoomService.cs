using Application.Dtos;
using Domain.Entities;
namespace Application.Abstraction
{
    public interface IRoomService
    {
        Task CreateRoomAsync(CreateRoomRequest request);
        Task<bool> UpdateRoomAsync(UpdateRoomRequest request);
        Task DeleteRoomAsync(Guid roomId);
        Task<Room> GetRoomDetailsByIdAsync(Guid roomId);
        Task<Room?> GetById(Guid roomId);
        IEnumerable<Room> SearchRoomsAsync(SearchRoomRequest request);
        IQueryable<Room> GetAllAsync();
    }
}
