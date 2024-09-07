namespace Application.Abstraction
{
    public interface ITrendingService
    {
        Task<IEnumerable<string>> GetTrendingCities();
    }
}
