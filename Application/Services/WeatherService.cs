using Application.Abstraction;
using Application.Dtos;
using System.Text.Json;

namespace Application.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "";
        private readonly IHotelService _hotelService;

        public WeatherService(HttpClient httpClient, IHotelService hotelService)
        {
            _httpClient = httpClient;
            _hotelService = hotelService;
        }

        public async Task<IEnumerable<Featured>> GetSuggestedCitiesBasedOnCurrentTemp(string cityName)
        {
            var temperature = await GetTemperatureAsync(cityName);
            var cities = await _hotelService.GetCities();
            IEnumerable<Featured> suggestedCities;

            if (temperature > 30)
            {
                suggestedCities = await FilterCitiesByTemperatureAsync(cities, maxTemperature: 30);
            }
            else if (temperature < 10)
            {
                suggestedCities = await FilterCitiesByTemperatureAsync(cities, minTemperature: 10);
            }
            else
            {
                suggestedCities = await FilterCitiesByTemperatureAsync(cities);
            }

            return suggestedCities;
        }

        private async Task<double> GetTemperatureAsync(string cityName)
        {
            var requestUrl = $"https://api.openweathermap.org/data/2.5/weather?q={Uri.EscapeDataString(cityName)}&appid={ApiKey}&units=metric";
            var response = await _httpClient.GetAsync(requestUrl);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error fetching weather data: {response.ReasonPhrase}. Details: {errorMessage}");
            }

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonDocument.Parse(json);
            return data.RootElement.GetProperty("main").GetProperty("temp").GetDouble();
        }

        private async Task<IEnumerable<Featured>> FilterCitiesByTemperatureAsync(IEnumerable<string> cities, double? minTemperature = null, double? maxTemperature = null)
        {
            var filteredCities = new List<Featured>();

            foreach (var city in cities)
            {
                try
                {
                    var cityTemperature = await GetTemperatureAsync(city);
                    if ((minTemperature == null || cityTemperature >= minTemperature) &&
                        (maxTemperature == null || cityTemperature <= maxTemperature))
                    {
                        filteredCities.Add(new Featured
                        {
                            City = city,
                            Temprature = cityTemperature
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to get temperature for city '{city}': {ex.Message}");
                }
            }

            return filteredCities;
        }
    }
}
