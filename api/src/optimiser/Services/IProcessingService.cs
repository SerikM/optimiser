using System.Collections.Generic;
using System.Threading.Tasks;
using Optimiser.Models;

namespace Optimiser.Services
{
    public interface IProcessingService
    {
        Task<List<Break>> GetDefaultData();
        Task<List<Break>> GetOptimalRatings(List<Break> breaks = null);
        bool SeedData();
    }
}
