using Application.Abstraction;
using Application.Dtos;
using Domain.Abstractions;
using Domain.Entities;


public class RoomService : IRoomService
{
	private readonly IRoomRepository _roomRepository;
	public RoomService(IRoomRepository roomRepository)
	{
        _roomRepository=roomRepository;

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

    public async Task DeleteRoomAsync(int roomId)
    {
        var room = _roomRepository.GetByIdAsync(roomId);
        if (room == null)
        {
            throw new KeyNotFoundException("Room not found");
        }
        await _roomRepository.DeleteAsync(room.Result.RoomId);
    }

    public IQueryable<Room> GetAllAsync()
    {
       return  _roomRepository.GetAllAsync();
    }

   
    public async Task<Room> GetById(int roomId)
    {
        var room = await _roomRepository.GetByIdAsync(roomId);
        if ( room != null)
        {
            return room;
        }
        return null;
    }

    public Task<Room> GetRoomDetailsByIdAsync(int roomId)
    {
        var room = _roomRepository.GetByIdAsync(roomId);
        if (room == null)
        {
            throw new KeyNotFoundException("Room not found");
        }
        return room; ;
    }


    public  async Task<IEnumerable<Room>> SearchRoomsAsync(SearchRoomRequest request)
    {
        // Start with the base query
        var query =  _roomRepository.GetAllAsync(); // Assuming this returns IQueryable<Room>

        // Apply filtering based on RoomType if specified
        if (request.RoomType.HasValue)
        {
            query =  query.Where(r => r.RoomType == request.RoomType.Value);
        }

        // Apply filtering based on CheckInDate and CheckOutDate if specified
        if (request.CheckInDate.HasValue && request.CheckOutDate.HasValue)
        {
            var checkInDate = request.CheckInDate.Value;
            var checkOutDate = request.CheckOutDate.Value;

            query = query.Where(r =>
                r.Reservations == null || !r.Reservations.Any(reservation =>
                    reservation.CheckInDate < checkOutDate && reservation.CheckOutDate > checkInDate));
        }

        // Apply filtering based on NumberOfChildren if specified
        if (request.NumberOfChildren.HasValue)
        {
            query = query.Where(r => r.NumberOfChildren >= request.NumberOfChildren.Value);
        }

        // Apply filtering based on NumberOfAdults if specified
        if (request.NumberOfAdults.HasValue)
        {
            query = query.Where(r => r.NumberOfAdults >= request.NumberOfAdults.Value);
        }

        // Apply filtering based on MinPricePerNight if specified
        if (request.MinPricePerNight.HasValue)
        {
            query = query.Where(r => r.PricePerNight >= request.MinPricePerNight.Value);
        }

        // Apply filtering based on MaxPricePerNight if specified
        if (request.MaxPricePerNight.HasValue)
        {
            query = query.Where(r => r.PricePerNight <= request.MaxPricePerNight.Value);
        }

        // Execute the query and get the results
        var rooms =  query.ToList();

        // Check if the result set meets the required number of rooms
        if (rooms.Count() >= request.NumberOfRooms)
        {
            return rooms;
        }

        return new List<Room>();
    }


    public async Task UpdateRoomAsync(UpdateRoomRequest request)
    {
        var room = _roomRepository.GetByIdAsync(request.RoomId);
        if (room == null)
        {
            throw new KeyNotFoundException("Room not found");
        }
        await _roomRepository.UpdateAsync(room.Result);
    }

}
