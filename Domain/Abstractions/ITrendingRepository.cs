namespace Domain.Abstractions
{
    public interface ITrendingRepository
    {
        Task<IEnumerable<string>> GetTrendingCities();
    }
}
