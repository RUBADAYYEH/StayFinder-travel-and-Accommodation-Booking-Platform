using Application.Dtos;

namespace Application.Abstraction
{
    public interface IWeatherService
    {
        Task<IEnumerable<Featured>> GetSuggestedCitiesBasedOnCurrentTemp(string cityName);
    }
}
