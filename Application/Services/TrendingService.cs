using Application.Abstraction;
using Domain.Abstractions;

namespace Application.Services
{
    public class TrendingService : ITrendingService
    {
        private readonly ITrendingRepository _trendingRepository;

        public TrendingService(ITrendingRepository trendingRepository)
        {
            _trendingRepository = trendingRepository;
        }

        public async Task<IEnumerable<string>> GetTrendingCities()
        {
            return await _trendingRepository.GetTrendingCities();
        }
    }
}
