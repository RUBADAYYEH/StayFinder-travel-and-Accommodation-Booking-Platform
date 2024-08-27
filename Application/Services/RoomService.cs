using Application.Abstraction;
using Application.Dtos;
using Domain.Abstractions;
using Domain.Entities;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;
    public RoomService(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;

    }
    public async Task CreateRoomAsync(CreateRoomRequest request)
    {
        if (await _roomRepository.GetByIdAsync(request.RoomId) != null)
        {
            throw new HttpRequestException("Existing Room with id found");
        }
        var room = new Room
        {
            RoomId = request.RoomId,
            HotelId = request.HotelId,
            RoomType = request.RoomType,
            NumberOfAdults = request.NumberOfAdults,
            NumberOfChildren = request.NumberOfChildren,
            IsPetFriendly = request.IsPetFriendly,
            PricePerNight = request.PricePerNight,
            Description = request.Description,
            ThumbnailUrl = request.ThumbnailUrl
        };
        await _roomRepository.AddAsync(room);
    }

    public async Task DeleteRoomAsync(Guid roomId)
    {
        var room = _roomRepository.GetByIdAsync(roomId);
        if (room == null)
        {
            throw new KeyNotFoundException("Room not found");
        }
        await _roomRepository.DeleteAsync(room.Result!.RoomId);
    }
    public IQueryable<Room> GetAllAsync()
    {
        return _roomRepository.GetAllAsync();
    }

    public async Task<Room?> GetById(Guid roomId)
    {
        var room = await _roomRepository.GetByIdAsync(roomId);
        if (room != null)
        {
            return room;
        }
        return null;
    }

    public Task<Room> GetRoomDetailsByIdAsync(Guid roomId)
    {
        var room = _roomRepository.GetByIdAsync(roomId);
        if (room == null)
        {
            throw new KeyNotFoundException("Room not found");
        }
        return room!; 
    }

    public IEnumerable<Room> SearchRoomsAsync(SearchRoomRequest request)
    {
        var query = _roomRepository.GetAllAsync();
        if (request.RoomType.HasValue)
        {
            query = query.Where(r => r.RoomType == request.RoomType.Value);
        }
        if (request.CheckInDate.HasValue && request.CheckOutDate.HasValue)
        {
            var checkInDate = request.CheckInDate.Value;
            var checkOutDate = request.CheckOutDate.Value;

            query = query.Where(r =>
                r.Reservations == null || !r.Reservations.Any(reservation =>
                    reservation.CheckInDate < checkOutDate && reservation.CheckOutDate > checkInDate));
        }
        if (request.NumberOfChildren.HasValue)
        {
            query = query.Where(r => r.NumberOfChildren >= request.NumberOfChildren.Value);
        }
        if (request.NumberOfAdults.HasValue)
        {
            query = query.Where(r => r.NumberOfAdults >= request.NumberOfAdults.Value);
        }
        if (request.MinPricePerNight.HasValue)
        {
            query = query.Where(r => r.PricePerNight >= request.MinPricePerNight.Value);
        }
        if (request.MaxPricePerNight.HasValue)
        {
            query = query.Where(r => r.PricePerNight <= request.MaxPricePerNight.Value);
        }

        if (query.Count() >= request.NumberOfRooms)
        {
            return query;
        }
        return new List<Room>();
    }
    async Task<bool> IRoomService.UpdateRoomAsync(UpdateRoomRequest request)
    {
        var room = await _roomRepository.GetByIdAsync(request.RoomId);
        if (room == null)
        {
            throw new KeyNotFoundException("Room not found");
        }
        if (request.RoomType.HasValue)
            room.RoomType = request.RoomType.Value;
        if (request.NumberOfAdults.HasValue)
            room.NumberOfAdults = request.NumberOfAdults.Value;
        if (request.NumberOfChildren.HasValue)
            room.NumberOfChildren = request.NumberOfChildren.Value;
        if (request.IsPetFriendly.HasValue)
            room.IsPetFriendly = request.IsPetFriendly.Value;
        if (request.PricePerNight.HasValue)
            room.PricePerNight = request.PricePerNight.Value;
        if (!string.IsNullOrEmpty(request.Description))
            room.Description = request.Description;
        if (!string.IsNullOrEmpty(request.ThumbnailUrl))
            room.ThumbnailUrl = request.ThumbnailUrl;
        await _roomRepository.UpdateAsync(room);
        return true;
    }
}
