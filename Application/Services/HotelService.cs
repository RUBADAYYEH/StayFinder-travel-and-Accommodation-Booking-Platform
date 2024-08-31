using Application.Abstraction;
using Application.Dtos;
using Domain.Abstractions;
using Domain.Entities;

public class HotelService : IHotelService
{
    private readonly IHotelRepository _hotelRepository;

    public HotelService(IHotelRepository hotelRepository)
    {
        _hotelRepository = hotelRepository;
    }
    public async Task CreateHotelAsync(CreateHotelRequest request)
    {
        var hotel = await _hotelRepository.GetByIdAsync(request.HotelId);
        if (hotel != null)
        {
            throw new InvalidOperationException("Hotel with id already exist");
        }
        var finalHotel = new Hotel
        {
            HotelId = request.HotelId,
            HotelName = request.HotelName,
            OwnerId = request.OwnerId,
            HotelDescription = request.HotelDescription,
            RoomCount = request.RoomCount,
            ThumbnailUrl = request.ThumbnailUrl,
            StarRating = request.StarRating,
            Address = request.Address,
            City = request.City
        };
        await _hotelRepository.AddAsync(hotel);
    }

    public async Task<bool> UpdateHotelAsync(UpdateHotelRequest updateRequest)
    {

        var hotel = await _hotelRepository.GetByIdAsync(updateRequest.HotelId);
        if (hotel == null)
        {
            throw new KeyNotFoundException("Hotel not found");
        }


        if (!string.IsNullOrEmpty(updateRequest.HotelName))
            hotel.HotelName = updateRequest.HotelName;

        if (!string.IsNullOrEmpty(updateRequest.HotelDescription))
            hotel.HotelDescription = updateRequest.HotelDescription;

        if (updateRequest.RoomCount != 0)
            hotel.RoomCount = updateRequest.RoomCount;

        if (!string.IsNullOrEmpty(updateRequest.ThumbnailUrl))
            hotel.ThumbnailUrl = updateRequest.ThumbnailUrl;

        if (updateRequest.StarRating != 0)
            hotel.StarRating = updateRequest.StarRating;

        if (!string.IsNullOrEmpty(updateRequest.Address))
            hotel.Address = updateRequest.Address;

        if (!string.IsNullOrEmpty(updateRequest.City))
            hotel.City = updateRequest.City;

        await _hotelRepository.UpdateAsync(hotel);

        return true; //update was successful
    }

    public async Task DeleteHotelAsync(Guid hotelId)
    {
        var hotel =await _hotelRepository.GetByIdAsync(hotelId);
        if (hotel == null) throw new KeyNotFoundException("Hotel not found");

        await _hotelRepository.DeleteAsync(hotelId);
    }

    public async Task<Hotel?> GetHotelDetailsByIdAsync(Guid hotelId)
    {
        var hotel = await _hotelRepository.GetByIdAsync(hotelId);
        if (hotel == null) throw new KeyNotFoundException("Hotel not found");
        return hotel;
    }

    public async Task<IEnumerable<Hotel>> GetHotelsByCityAsync(string city)
    {
        return await _hotelRepository.GetByCityAsync(city);
    }

    public Task<IEnumerable<Hotel>> GetAllAsync()
    {
        return _hotelRepository.GetAllAsync();
    }

    public async Task<Hotel?> GetById(Guid hotelId)
    {
        var hotel = await _hotelRepository.GetByIdAsync(hotelId);
        if (hotel != null)
        {
            return hotel;
        }
        return null;
    }

    public async Task<IEnumerable<Room>?> GetRoomsForHotelId(Guid hotelId)
    {
        var rooms = await _hotelRepository.GetRoomsForHotelId(hotelId);
        if (rooms.Any())
        {
            return rooms;
        }
        return new List<Room>();
    }

    public async Task<IEnumerable<string>> GetCities()
    {
        var res = await _hotelRepository.GetCities();
        return res;
    }
}
