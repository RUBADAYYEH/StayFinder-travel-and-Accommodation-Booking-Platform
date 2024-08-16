using Application.Dtos;
using Domain.Entities;
namespace Application.Abstraction
{
    public interface IRoomService
    {
        Task CreateRoomAsync(CreateRoomRequest request);
        Task UpdateRoomAsync(UpdateRoomRequest request);
        Task DeleteRoomAsync(int roomId);
        Task<Room> GetRoomDetailsByIdAsync(int roomId);
        Task<Room> GetById(int roomId);
        Task<IEnumerable<Room>> SearchRoomsAsync(SearchRoomRequest request);
        IQueryable<Room> GetAllAsync();
       
    }
}
